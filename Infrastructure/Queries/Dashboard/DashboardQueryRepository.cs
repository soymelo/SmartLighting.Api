using Dapper;
using Microsoft.Data.SqlClient;
using SmartLighting.Api.Application.Abstractions.Queries.Dashboard;
using SmartLighting.Api.Application.ReadModels.Dashboard;

namespace SmartLighting.Api.Infrastructure.Queries.Dashboard;

public sealed class DashboardQueryRepository(IConfiguration configuration) : IDashboardQueryRepository
{
    private const string DashboardSql = """
        SELECT d.Id, d.Name, d.LastTelemetryAtUtc
        FROM Devices AS d
        WHERE d.Id = @DeviceId;

        SELECT l.Id AS LuminaireId,
               l.Name,
               z.Name AS ZoneName,
               ls.BrightnessPercentage,
               ls.UpdatedAtUtc
        FROM Luminaires AS l
        INNER JOIN Zones AS z ON z.Id = l.ZoneId
        LEFT JOIN LuminaireStates AS ls ON ls.LuminaireId = l.Id
        WHERE l.DeviceId = @DeviceId
        ORDER BY z.DisplayOrder, l.Name, l.Id;

        SELECT TOP (50) se.EventIdentifier,
                        se.SensorId,
                        s.Name AS SensorName,
                        z.Name AS ZoneName,
                        se.OccurredAtUtc
        FROM SensorEvents AS se
        INNER JOIN Sensors AS s ON s.Id = se.SensorId
        INNER JOIN Zones AS z ON z.Id = s.ZoneId
        WHERE se.DeviceId = @DeviceId
        ORDER BY se.OccurredAtUtc DESC, se.EventIdentifier DESC;
        """;

    private readonly string _connectionString = configuration.GetConnectionString("SmartLightingDatabase")
        ?? throw new InvalidOperationException("A connection string 'SmartLightingDatabase' não foi configurada.");

    public async Task<DeviceDashboardDto?> GetDashboardByIdAsync(
        Guid deviceId,
        CancellationToken ct)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(ct);

        var command = new CommandDefinition(
            DashboardSql,
            new { DeviceId = deviceId },
            cancellationToken: ct);

        using var results = await connection.QueryMultipleAsync(command);

        var device = await results.ReadSingleOrDefaultAsync<DeviceRow>();

        if (device is null)
        {
            return null;
        }

        var luminaires = (await results.ReadAsync<DashboardLuminaireRow>())
            .Select(luminaire => new DeviceDashboardLuminaireDto(
                luminaire.LuminaireId,
                luminaire.Name,
                luminaire.ZoneName,
                luminaire.BrightnessPercentage,
                ToUtc(luminaire.UpdatedAtUtc)))
            .ToList();

        var sensorEvents = (await results.ReadAsync<DashboardSensorEventRow>())
            .Select(sensorEvent => new DeviceDashboardSensorEventDto(
                sensorEvent.EventIdentifier,
                sensorEvent.SensorId,
                sensorEvent.SensorName,
                sensorEvent.ZoneName,
                DateTime.SpecifyKind(sensorEvent.OccurredAtUtc, DateTimeKind.Utc)))
            .ToList();

        return new DeviceDashboardDto(
            device.Id,
            device.Name,
            ToUtc(device.LastTelemetryAtUtc),
            luminaires,
            sensorEvents);
    }

    private static DateTime? ToUtc(DateTime? value) => value is null
        ? null
        : DateTime.SpecifyKind(value.Value, DateTimeKind.Utc);

    private sealed class DeviceRow
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public DateTime? LastTelemetryAtUtc { get; init; }
    }

    private sealed class DashboardLuminaireRow
    {
        public Guid LuminaireId { get; init; }
        public string Name { get; init; } = string.Empty;
        public string ZoneName { get; init; } = string.Empty;
        public int? BrightnessPercentage { get; init; }
        public DateTime? UpdatedAtUtc { get; init; }
    }

    private sealed class DashboardSensorEventRow
    {
        public Guid EventIdentifier { get; init; }
        public Guid SensorId { get; init; }
        public string SensorName { get; init; } = string.Empty;
        public string ZoneName { get; init; } = string.Empty;
        public DateTime OccurredAtUtc { get; init; }
    }
}
