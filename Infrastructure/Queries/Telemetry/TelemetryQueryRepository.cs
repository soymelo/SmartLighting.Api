using Dapper;
using Microsoft.Data.SqlClient;
using SmartLighting.Api.Application.Abstractions.Queries.Telemetry;
using SmartLighting.Api.Application.ReadModels.Telemetry;

namespace SmartLighting.Api.Infrastructure.Queries.Telemetry;

public sealed class TelemetryQueryRepository(IConfiguration configuration) : ITelemetryQueryRepository
{
    private const string TelemetryStatusSql = """
        SELECT d.Id, d.LastTelemetryAtUtc
        FROM Devices AS d
        WHERE d.Id = @DeviceId;
        """;

    private const string LuminaireStatesSql = """
        SELECT d.Id, d.Name
        FROM Devices AS d
        WHERE d.Id = @DeviceId;

        SELECT l.Id AS LuminaireId, ls.BrightnessPercentage, ls.UpdatedAtUtc
        FROM Luminaires AS l
        INNER JOIN Zones AS z ON z.Id = l.ZoneId
        LEFT JOIN LuminaireStates AS ls ON ls.LuminaireId = l.Id
        WHERE l.DeviceId = @DeviceId
        ORDER BY z.DisplayOrder, l.Name, l.Id;
        """;

    private const string RecentSensorEventsSql = """
        SELECT d.Id, d.Name
        FROM Devices AS d
        WHERE d.Id = @DeviceId;

        SELECT TOP (@Limit) se.EventIdentifier, se.SensorId, se.OccurredAtUtc
        FROM SensorEvents AS se
        WHERE se.DeviceId = @DeviceId
        ORDER BY se.OccurredAtUtc DESC, se.EventIdentifier DESC;
        """;

    private readonly string _connectionString = configuration.GetConnectionString("SmartLightingDatabase")
        ?? throw new InvalidOperationException("A connection string 'SmartLightingDatabase' não foi configurada.");

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

        return new DeviceTelemetryStatusDto(device.Id, ToUtc(device.LastTelemetryAtUtc));
    }

    public async Task<DeviceLuminaireStatesDto?> GetLuminaireStatesByDeviceIdAsync(
        Guid deviceId,
        CancellationToken ct)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(ct);

        var command = new CommandDefinition(
            LuminaireStatesSql,
            new { DeviceId = deviceId },
            cancellationToken: ct);

        using var results = await connection.QueryMultipleAsync(command);

        var device = await results.ReadSingleOrDefaultAsync<DeviceRow>();

        if (device is null)
        {
            return null;
        }

        var luminaires = (await results.ReadAsync<LuminaireStateRow>())
            .Select(luminaire => new DeviceLuminaireStateDto(
                luminaire.LuminaireId,
                luminaire.BrightnessPercentage,
                ToUtc(luminaire.UpdatedAtUtc)))
            .ToList();

        return new DeviceLuminaireStatesDto(device.Id, luminaires);
    }

    public async Task<DeviceRecentSensorEventsDto?> GetRecentSensorEventsByDeviceIdAsync(
        Guid deviceId,
        int limit,
        CancellationToken ct)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(ct);

        var command = new CommandDefinition(
            RecentSensorEventsSql,
            new { DeviceId = deviceId, Limit = limit },
            cancellationToken: ct);

        using var results = await connection.QueryMultipleAsync(command);

        var device = await results.ReadSingleOrDefaultAsync<DeviceRow>();

        if (device is null)
        {
            return null;
        }

        var events = (await results.ReadAsync<SensorEventRow>())
            .Select(sensorEvent => new DeviceRecentSensorEventDto(
                sensorEvent.EventIdentifier,
                sensorEvent.SensorId,
                DateTime.SpecifyKind(sensorEvent.OccurredAtUtc, DateTimeKind.Utc)))
            .ToList();

        return new DeviceRecentSensorEventsDto(device.Id, events);
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

    private sealed class DeviceTelemetryStatusRow
    {
        public Guid Id { get; init; }
        public DateTime? LastTelemetryAtUtc { get; init; }
    }

    private sealed class LuminaireStateRow
    {
        public Guid LuminaireId { get; init; }
        public int? BrightnessPercentage { get; init; }
        public DateTime? UpdatedAtUtc { get; init; }
    }

    private sealed class SensorEventRow
    {
        public Guid EventIdentifier { get; init; }
        public Guid SensorId { get; init; }
        public DateTime OccurredAtUtc { get; init; }
    }
}
