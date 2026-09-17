using Dapper;
using Microsoft.Data.SqlClient;
using SmartLighting.Api.Application.Abstractions.Queries;
using SmartLighting.Api.Application.ReadModels.Devices;

namespace SmartLighting.Api.Infrastructure.Queries;

public sealed class DeviceQueryRepository(IConfiguration configuration) : IDeviceQueryRepository
{
    private const string Sql = """
        SELECT d.DeviceIdentifier
        FROM Devices AS d
        WHERE d.DeviceIdentifier = @DeviceIdentifier;

        SELECT z.Id, z.ZoneIdentifier, z.DisplayOrder
        FROM Zones AS z
        INNER JOIN Devices AS d ON d.Id = z.DeviceId
        WHERE d.DeviceIdentifier = @DeviceIdentifier
        ORDER BY z.DisplayOrder;

        SELECT s.ZoneId, s.SensorIdentifier
        FROM Sensors AS s
        INNER JOIN Devices AS d ON d.Id = s.DeviceId
        WHERE d.DeviceIdentifier = @DeviceIdentifier
        ORDER BY s.ZoneId, s.SensorIdentifier;

        SELECT l.ZoneId, l.LuminaireIdentifier, l.NominalPowerWatts
        FROM Luminaires AS l
        INNER JOIN Zones AS z ON z.Id = l.ZoneId
        INNER JOIN Devices AS d ON d.Id = z.DeviceId
        WHERE d.DeviceIdentifier = @DeviceIdentifier
        ORDER BY l.ZoneId, l.LuminaireIdentifier;
        """;

    private readonly string _connectionString = configuration.GetConnectionString("SmartLightingDatabase")
        ?? throw new InvalidOperationException("A connection string 'SmartLightingDatabase' não foi configurada.");

    public async Task<DeviceTopologyDto?> GetTopologyByIdentifierAsync(
        string deviceIdentifier,
        CancellationToken ct)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(ct);

        var command = new CommandDefinition(
            Sql,
            new { DeviceIdentifier = deviceIdentifier },
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
                    .Select(sensor => new DeviceTopologySensorDto(sensor.SensorIdentifier))
                    .ToList());
        var luminairesByZone = (await results.ReadAsync<LuminaireRow>())
            .GroupBy(luminaire => luminaire.ZoneId)
            .ToDictionary(
                group => group.Key,
                group => (IReadOnlyCollection<DeviceTopologyLuminaireDto>)group
                    .Select(luminaire => new DeviceTopologyLuminaireDto(
                        luminaire.LuminaireIdentifier,
                        luminaire.NominalPowerWatts))
                    .ToList());

        var zoneDtos = zones
            .Select(zone => new DeviceTopologyZoneDto(
                zone.ZoneIdentifier,
                zone.DisplayOrder,
                sensorsByZone.GetValueOrDefault(zone.Id, Array.Empty<DeviceTopologySensorDto>()),
                luminairesByZone.GetValueOrDefault(zone.Id, Array.Empty<DeviceTopologyLuminaireDto>())))
            .ToList();

        return new DeviceTopologyDto(device.DeviceIdentifier, zoneDtos);
    }

    private sealed class DeviceRow
    {
        public string DeviceIdentifier { get; init; } = string.Empty;
    }

    private sealed class ZoneRow
    {
        public Guid Id { get; init; }
        public string ZoneIdentifier { get; init; } = string.Empty;
        public int DisplayOrder { get; init; }
    }

    private sealed class SensorRow
    {
        public Guid ZoneId { get; init; }
        public string SensorIdentifier { get; init; } = string.Empty;
    }

    private sealed class LuminaireRow
    {
        public Guid ZoneId { get; init; }
        public string LuminaireIdentifier { get; init; } = string.Empty;
        public decimal NominalPowerWatts { get; init; }
    }
}
