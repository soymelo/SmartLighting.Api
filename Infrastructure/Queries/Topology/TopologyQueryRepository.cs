using Dapper;
using Microsoft.Data.SqlClient;
using SmartLighting.Api.Application.Abstractions.Queries.Topology;
using SmartLighting.Api.Application.ReadModels.Topology;
using SmartLighting.Api.Domain.Hardware;

namespace SmartLighting.Api.Infrastructure.Queries.Topology;

public sealed class TopologyQueryRepository(IConfiguration configuration) : ITopologyQueryRepository
{
    private const string TopologySql = """
        SELECT d.Id, d.Name, d.ProjectId, p.Name AS ProjectName, d.LastTelemetryAtUtc
        FROM Devices AS d
        INNER JOIN Projects AS p ON p.Id = d.ProjectId
        WHERE d.Id = @DeviceId;

        SELECT z.Id, z.Name, z.DisplayOrder
        FROM Zones AS z
        WHERE EXISTS (SELECT 1 FROM Sensors AS s WHERE s.ZoneId = z.Id AND s.DeviceId = @DeviceId)
           OR EXISTS (
               SELECT 1
               FROM ZoneLuminaires AS zl
               INNER JOIN Luminaires AS l ON l.Id = zl.LuminaireId
               WHERE zl.ZoneId = z.Id AND l.DeviceId = @DeviceId)
        ORDER BY z.DisplayOrder, z.Id;

        SELECT s.Id, s.ZoneId, s.Name, s.Slot
        FROM Sensors AS s
        WHERE s.DeviceId = @DeviceId
        ORDER BY s.ZoneId, s.Slot;

        SELECT l.Id, zl.ZoneId, l.Name, l.NominalPowerWatts, l.Slot
        FROM Luminaires AS l
        INNER JOIN ZoneLuminaires AS zl ON zl.LuminaireId = l.Id
        WHERE l.DeviceId = @DeviceId
        ORDER BY zl.ZoneId, l.Slot;
        """;

    private const string ProjectControlTopologySql = """
        DECLARE @ProjectId uniqueidentifier = (SELECT ProjectId FROM Devices WHERE Id = @DeviceId);

        SELECT d.Id, d.Name, d.ProjectId
        FROM Devices AS d
        WHERE d.Id = @DeviceId;

        SELECT z.Id, z.Name, z.DisplayOrder
        FROM Zones AS z
        WHERE z.ProjectId = @ProjectId
        ORDER BY z.DisplayOrder, z.Id;

        SELECT s.Id, s.DeviceId, s.ZoneId, s.Name, s.Slot
        FROM Sensors AS s
        INNER JOIN Devices AS d ON d.Id = s.DeviceId
        INNER JOIN Zones AS z ON z.Id = s.ZoneId
        WHERE d.ProjectId = @ProjectId AND z.ProjectId = @ProjectId
        ORDER BY s.ZoneId, s.DeviceId, s.Slot;

        SELECT l.Id, l.DeviceId, zl.ZoneId, l.Name, l.NominalPowerWatts, l.Slot
        FROM Luminaires AS l
        INNER JOIN Devices AS d ON d.Id = l.DeviceId
        INNER JOIN ZoneLuminaires AS zl ON zl.LuminaireId = l.Id
        INNER JOIN Zones AS z ON z.Id = zl.ZoneId
        WHERE d.ProjectId = @ProjectId AND z.ProjectId = @ProjectId
        ORDER BY zl.ZoneId, l.DeviceId, l.Slot;
        """;

    private readonly string _connectionString = configuration.GetConnectionString("SmartLightingDatabase")
        ?? throw new InvalidOperationException("A connection string 'SmartLightingDatabase' não foi configurada.");

    public async Task<DeviceTopologyDto?> GetTopologyByIdAsync(
        Guid deviceId,
        CancellationToken ct)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(ct);

        var command = new CommandDefinition(
            TopologySql,
            new { DeviceId = deviceId },
            cancellationToken: ct);

        using var results = await connection.QueryMultipleAsync(command);

        var device = await results.ReadSingleOrDefaultAsync<DeviceRow>();

        if (device is null)
        {
            return null;
        }

        var zones = (await results.ReadAsync<ZoneRow>()).ToList();
        var sensorsByZone = (await results.ReadAsync<SensorRow>())
            .GroupBy(sensor => sensor.ZoneId)
            .ToDictionary(
                group => group.Key,
                group => (IReadOnlyCollection<DeviceTopologySensorDto>)group
                    .Select(sensor => new DeviceTopologySensorDto(
                        sensor.Id,
                        sensor.Name,
                        sensor.Slot,
                        Esp32Pinout.GetGpio(sensor.Slot)))
                    .ToList());
        var luminairesByZone = (await results.ReadAsync<LuminaireRow>())
            .GroupBy(luminaire => luminaire.ZoneId)
            .ToDictionary(
                group => group.Key,
                group => (IReadOnlyCollection<DeviceTopologyLuminaireDto>)group
                    .Select(luminaire => new DeviceTopologyLuminaireDto(
                        luminaire.Id,
                        luminaire.Name,
                        luminaire.NominalPowerWatts,
                        luminaire.Slot,
                        Esp32Pinout.GetGpio(luminaire.Slot)))
                    .ToList());

        var zoneDtos = zones
            .Select(zone => new DeviceTopologyZoneDto(
                zone.Id,
                zone.Name,
                zone.DisplayOrder,
                sensorsByZone.GetValueOrDefault(zone.Id, Array.Empty<DeviceTopologySensorDto>()),
                luminairesByZone.GetValueOrDefault(zone.Id, Array.Empty<DeviceTopologyLuminaireDto>())))
            .ToList();

        return new DeviceTopologyDto(
            device.Id,
            device.Name,
            device.ProjectId,
            device.ProjectName,
            device.LastTelemetryAtUtc is null
                ? null
                : DateTime.SpecifyKind(device.LastTelemetryAtUtc.Value, DateTimeKind.Utc),
            zoneDtos);
    }

    public async Task<ProjectControlTopologyDto?> GetProjectControlTopologyByDeviceIdAsync(
        Guid deviceId,
        CancellationToken ct)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(ct);

        using var results = await connection.QueryMultipleAsync(new CommandDefinition(
            ProjectControlTopologySql,
            new { DeviceId = deviceId },
            cancellationToken: ct));
        var device = await results.ReadSingleOrDefaultAsync<DeviceRow>();
        if (device is null)
        {
            return null;
        }

        var zones = (await results.ReadAsync<ZoneRow>()).ToList();
        var sensorsByZone = (await results.ReadAsync<ControlSensorRow>())
            .GroupBy(sensor => sensor.ZoneId)
            .ToDictionary(group => group.Key, group => (IReadOnlyCollection<ProjectControlSensorDto>)group
                .Select(sensor => new ProjectControlSensorDto(
                    sensor.Id,
                    sensor.DeviceId,
                    sensor.Name,
                    sensor.Slot,
                    Esp32Pinout.GetGpio(sensor.Slot)))
                .ToList());
        var luminairesByZone = (await results.ReadAsync<ControlLuminaireRow>())
            .GroupBy(luminaire => luminaire.ZoneId)
            .ToDictionary(group => group.Key, group => (IReadOnlyCollection<ProjectControlLuminaireDto>)group
                .Select(luminaire => new ProjectControlLuminaireDto(
                    luminaire.Id,
                    luminaire.DeviceId,
                    luminaire.Name,
                    luminaire.NominalPowerWatts,
                    luminaire.Slot,
                    Esp32Pinout.GetGpio(luminaire.Slot)))
                .ToList());

        return new ProjectControlTopologyDto(
            device.Id,
            device.Name,
            zones.Select(zone => new ProjectControlZoneDto(
                zone.Id,
                zone.Name,
                zone.DisplayOrder,
                sensorsByZone.GetValueOrDefault(zone.Id, Array.Empty<ProjectControlSensorDto>()),
                luminairesByZone.GetValueOrDefault(zone.Id, Array.Empty<ProjectControlLuminaireDto>())))
                .ToList());
    }

    private sealed class DeviceRow
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public Guid ProjectId { get; init; }
        public string ProjectName { get; init; } = string.Empty;
        public DateTime? LastTelemetryAtUtc { get; init; }
    }

    private sealed class ZoneRow
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public int DisplayOrder { get; init; }
    }

    private sealed class SensorRow
    {
        public Guid Id { get; init; }
        public Guid ZoneId { get; init; }
        public string Name { get; init; } = string.Empty;
        public SensorSlot Slot { get; init; }
    }

    private sealed class LuminaireRow
    {
        public Guid Id { get; init; }
        public Guid ZoneId { get; init; }
        public string Name { get; init; } = string.Empty;
        public decimal NominalPowerWatts { get; init; }
        public LuminaireSlot Slot { get; init; }
    }

    private sealed class ControlSensorRow
    {
        public Guid Id { get; init; }
        public Guid DeviceId { get; init; }
        public Guid ZoneId { get; init; }
        public string Name { get; init; } = string.Empty;
        public SensorSlot Slot { get; init; }
    }

    private sealed class ControlLuminaireRow
    {
        public Guid Id { get; init; }
        public Guid DeviceId { get; init; }
        public Guid ZoneId { get; init; }
        public string Name { get; init; } = string.Empty;
        public decimal NominalPowerWatts { get; init; }
        public LuminaireSlot Slot { get; init; }
    }
}
