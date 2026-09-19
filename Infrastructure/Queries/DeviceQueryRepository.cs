using Dapper;
using Microsoft.Data.SqlClient;
using SmartLighting.Api.Application.Abstractions.Queries;
using SmartLighting.Api.Application.ReadModels.Devices;

namespace SmartLighting.Api.Infrastructure.Queries;

public sealed class DeviceQueryRepository(IConfiguration configuration) : IDeviceQueryRepository
{
    private const string TopologySql = """
        SELECT d.Id, d.Name
        FROM Devices AS d
        WHERE d.Id = @DeviceId;

        SELECT z.Id, z.Name, z.DisplayOrder
        FROM Zones AS z
        WHERE z.DeviceId = @DeviceId
        ORDER BY z.DisplayOrder;

        SELECT s.Id, s.ZoneId, s.Name
        FROM Sensors AS s
        INNER JOIN Zones AS z ON z.Id = s.ZoneId
        WHERE z.DeviceId = @DeviceId
        ORDER BY s.ZoneId, s.Name;

        SELECT l.Id, l.ZoneId, l.Name, l.NominalPowerWatts
        FROM Luminaires AS l
        INNER JOIN Zones AS z ON z.Id = l.ZoneId
        WHERE z.DeviceId = @DeviceId
        ORDER BY l.ZoneId, l.Name;
        """;

    private const string TelemetryStatusSql = """
        SELECT d.Id, d.LastTelemetryAtUtc
        FROM Devices AS d
        WHERE d.Id = @DeviceId;
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
                    .Select(sensor => new DeviceTopologySensorDto(sensor.Id, sensor.Name))
                    .ToList());
        var luminairesByZone = (await results.ReadAsync<LuminaireRow>())
            .GroupBy(luminaire => luminaire.ZoneId)
            .ToDictionary(
                group => group.Key,
                group => (IReadOnlyCollection<DeviceTopologyLuminaireDto>)group
                    .Select(luminaire => new DeviceTopologyLuminaireDto(
                        luminaire.Id,
                        luminaire.Name,
                        luminaire.NominalPowerWatts))
                    .ToList());

        var zoneDtos = zones
            .Select(zone => new DeviceTopologyZoneDto(
                zone.Id,
                zone.Name,
                zone.DisplayOrder,
                sensorsByZone.GetValueOrDefault(zone.Id, Array.Empty<DeviceTopologySensorDto>()),
                luminairesByZone.GetValueOrDefault(zone.Id, Array.Empty<DeviceTopologyLuminaireDto>())))
            .ToList();

        return new DeviceTopologyDto(device.Id, device.Name, zoneDtos);
    }

    public async Task<DeviceTelemetryStatusDto?> GetTelemetryStatusByIdAsync(
        Guid deviceId,
        CancellationToken ct)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(ct);

        var command = new CommandDefinition(
            TelemetryStatusSql,
            new { DeviceId = deviceId },
            cancellationToken: ct);
        var device = await connection.QuerySingleOrDefaultAsync<DeviceTelemetryStatusRow>(command);

        if (device is null)
        {
            return null;
        }

        var lastTelemetryAtUtc = device.LastTelemetryAtUtc is null
            ? (DateTime?)null
            : DateTime.SpecifyKind(device.LastTelemetryAtUtc.Value, DateTimeKind.Utc);

        return new DeviceTelemetryStatusDto(device.Id, lastTelemetryAtUtc);
    }

    private sealed class DeviceRow
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
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
    }

    private sealed class LuminaireRow
    {
        public Guid Id { get; init; }
        public Guid ZoneId { get; init; }
        public string Name { get; init; } = string.Empty;
        public decimal NominalPowerWatts { get; init; }
    }

    private sealed class DeviceTelemetryStatusRow
    {
        public Guid Id { get; init; }
        public DateTime? LastTelemetryAtUtc { get; init; }
    }
}
