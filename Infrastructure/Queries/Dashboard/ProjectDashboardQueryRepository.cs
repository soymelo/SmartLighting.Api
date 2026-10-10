using Dapper;
using Microsoft.Data.SqlClient;
using SmartLighting.Api.Application.Abstractions.Queries.Dashboard;
using SmartLighting.Api.Application.ReadModels.Dashboard;

namespace SmartLighting.Api.Infrastructure.Queries.Dashboard;

public sealed class ProjectDashboardQueryRepository(IConfiguration configuration) : IProjectDashboardQueryRepository
{
    private const string DashboardSql = """
        SELECT p.Id, p.Name
        FROM Projects AS p
        WHERE p.Id = @ProjectId;

        SELECT d.Id AS DeviceId, d.Name, d.LastTelemetryAtUtc
        FROM Devices AS d
        WHERE d.ProjectId = @ProjectId
        ORDER BY d.Name, d.Id;

        SELECT z.Id AS ZoneId, z.Name, z.DisplayOrder
        FROM Zones AS z
        WHERE z.ProjectId = @ProjectId
        ORDER BY z.DisplayOrder, z.Id;

        SELECT s.Id AS SensorId, s.ZoneId, s.DeviceId, s.Name
        FROM Sensors AS s
        INNER JOIN Zones AS z ON z.Id = s.ZoneId
        INNER JOIN Devices AS d ON d.Id = s.DeviceId
        WHERE z.ProjectId = @ProjectId AND d.ProjectId = @ProjectId
        ORDER BY z.DisplayOrder, s.Name, s.Id;

        SELECT l.Id AS LuminaireId,
               zl.ZoneId,
               l.DeviceId,
               l.Name,
               l.NominalPowerWatts,
               ls.BrightnessPercentage,
               ls.UpdatedAtUtc
        FROM Luminaires AS l
        INNER JOIN ZoneLuminaires AS zl ON zl.LuminaireId = l.Id
        INNER JOIN Zones AS z ON z.Id = zl.ZoneId
        INNER JOIN Devices AS d ON d.Id = l.DeviceId
        LEFT JOIN LuminaireStates AS ls ON ls.LuminaireId = l.Id
        WHERE z.ProjectId = @ProjectId AND d.ProjectId = @ProjectId
        ORDER BY z.DisplayOrder, l.Name, l.Id;

        SELECT TOP (50) se.EventIdentifier,
                        se.SensorId,
                        se.DeviceId,
                        s.Name AS SensorName,
                        z.Id AS ZoneId,
                        z.Name AS ZoneName,
                        se.OccurredAtUtc
        FROM SensorEvents AS se
        INNER JOIN Devices AS d ON d.Id = se.DeviceId
        INNER JOIN Sensors AS s ON s.Id = se.SensorId
        INNER JOIN Zones AS z ON z.Id = s.ZoneId
        WHERE d.ProjectId = @ProjectId
        ORDER BY se.OccurredAtUtc DESC, se.EventIdentifier DESC;

        SELECT s.ZoneId, COUNT(*) AS EventCount
        FROM SensorEvents AS se
        INNER JOIN Sensors AS s ON s.Id = se.SensorId
        INNER JOIN Devices AS d ON d.Id = se.DeviceId
        WHERE d.ProjectId = @ProjectId
          AND s.DeviceId = se.DeviceId
          AND se.OccurredAtUtc >= @ActivitySinceUtc
          AND se.OccurredAtUtc <= @ActivityUntilUtc
        GROUP BY s.ZoneId;
        """;

    private readonly string _connectionString = configuration.GetConnectionString("SmartLightingDatabase")
        ?? throw new InvalidOperationException("A connection string 'SmartLightingDatabase' não foi configurada.");

    public async Task<ProjectDashboardDto?> GetProjectDashboardByIdAsync(
        Guid projectId,
        CancellationToken ct)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(ct);

        var nowUtc = DateTime.UtcNow;
        var command = new CommandDefinition(
            DashboardSql,
            new { ProjectId = projectId, ActivitySinceUtc = nowUtc.AddHours(-24), ActivityUntilUtc = nowUtc },
            cancellationToken: ct);

        using var results = await connection.QueryMultipleAsync(command);

        var project = await results.ReadSingleOrDefaultAsync<ProjectRow>();

        if (project is null)
        {
            return null;
        }

        var devices = (await results.ReadAsync<DeviceRow>())
            .Select(device => new ProjectDashboardDeviceDto(
                device.DeviceId,
                device.Name,
                ToUtc(device.LastTelemetryAtUtc)))
            .ToList();

        var zones = (await results.ReadAsync<ZoneRow>()).ToList();
        var sensorsByZone = (await results.ReadAsync<SensorRow>())
            .GroupBy(sensor => sensor.ZoneId)
            .ToDictionary(
                group => group.Key,
                group => (IReadOnlyCollection<ProjectDashboardSensorDto>)group
                    .Select(sensor => new ProjectDashboardSensorDto(
                        sensor.SensorId,
                        sensor.DeviceId,
                        sensor.Name))
                    .ToList());
        var luminairesByZone = (await results.ReadAsync<LuminaireRow>())
            .GroupBy(luminaire => luminaire.ZoneId)
            .ToDictionary(
                group => group.Key,
                group => (IReadOnlyCollection<ProjectDashboardLuminaireDto>)group
                    .Select(luminaire => new ProjectDashboardLuminaireDto(
                        luminaire.LuminaireId,
                        luminaire.DeviceId,
                        luminaire.Name,
                        luminaire.NominalPowerWatts,
                        luminaire.BrightnessPercentage,
                        ToUtc(luminaire.UpdatedAtUtc)))
                    .ToList());

        var zoneDtos = zones
            .Select(zone => new ProjectDashboardZoneDto(
                zone.ZoneId,
                zone.Name,
                zone.DisplayOrder,
                sensorsByZone.GetValueOrDefault(zone.ZoneId, Array.Empty<ProjectDashboardSensorDto>()),
                luminairesByZone.GetValueOrDefault(zone.ZoneId, Array.Empty<ProjectDashboardLuminaireDto>())))
            .ToList();

        var sensorEvents = (await results.ReadAsync<SensorEventRow>())
            .Select(sensorEvent => new ProjectDashboardSensorEventDto(
                sensorEvent.EventIdentifier,
                sensorEvent.SensorId,
                sensorEvent.DeviceId,
                sensorEvent.SensorName,
                sensorEvent.ZoneId,
                sensorEvent.ZoneName,
                DateTime.SpecifyKind(sensorEvent.OccurredAtUtc, DateTimeKind.Utc)))
            .ToList();

        var activity = (await results.ReadAsync<ZoneActivityRow>())
            .Select(row => new ProjectDashboardZoneActivityDto(row.ZoneId, row.EventCount))
            .ToList();

        return new ProjectDashboardDto(project.Id, project.Name, devices, zoneDtos, sensorEvents, activity);
    }

    private static DateTime? ToUtc(DateTime? value) => value is null
        ? null
        : DateTime.SpecifyKind(value.Value, DateTimeKind.Utc);

    private sealed class ProjectRow
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }

    private sealed class DeviceRow
    {
        public Guid DeviceId { get; init; }
        public string Name { get; init; } = string.Empty;
        public DateTime? LastTelemetryAtUtc { get; init; }
    }

    private sealed class ZoneRow
    {
        public Guid ZoneId { get; init; }
        public string Name { get; init; } = string.Empty;
        public int DisplayOrder { get; init; }
    }

    private sealed class SensorRow
    {
        public Guid SensorId { get; init; }
        public Guid ZoneId { get; init; }
        public Guid DeviceId { get; init; }
        public string Name { get; init; } = string.Empty;
    }

    private sealed class LuminaireRow
    {
        public Guid LuminaireId { get; init; }
        public Guid ZoneId { get; init; }
        public Guid DeviceId { get; init; }
        public string Name { get; init; } = string.Empty;
        public decimal NominalPowerWatts { get; init; }
        public int? BrightnessPercentage { get; init; }
        public DateTime? UpdatedAtUtc { get; init; }
    }

    private sealed class SensorEventRow
    {
        public Guid EventIdentifier { get; init; }
        public Guid SensorId { get; init; }
        public Guid DeviceId { get; init; }
        public string SensorName { get; init; } = string.Empty;
        public Guid ZoneId { get; init; }
        public string ZoneName { get; init; } = string.Empty;
        public DateTime OccurredAtUtc { get; init; }
    }

    private sealed class ZoneActivityRow
    {
        public Guid ZoneId { get; init; }
        public int EventCount { get; init; }
    }
}
