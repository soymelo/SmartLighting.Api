using Dapper;
using Microsoft.Data.SqlClient;
using SmartLighting.Api.Application.Abstractions.Queries.Devices;
using SmartLighting.Api.Application.ReadModels.Devices;

namespace SmartLighting.Api.Infrastructure.Queries.Devices;

public sealed class DeviceQueryRepository(IConfiguration configuration) : IDeviceQueryRepository
{
    private const string DevicesSql = """
        SELECT d.Id, d.Name, d.ProjectId, p.Name AS ProjectName, d.LastTelemetryAtUtc
        FROM Devices AS d
        INNER JOIN Projects AS p ON p.Id = d.ProjectId
        ORDER BY d.Name, d.Id;
        """;

    private readonly string _connectionString = configuration.GetConnectionString("SmartLightingDatabase")
        ?? throw new InvalidOperationException("A connection string 'SmartLightingDatabase' não foi configurada.");

    public async Task<IReadOnlyCollection<DeviceSummaryDto>> GetAllAsync(CancellationToken ct)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(ct);

        var command = new CommandDefinition(DevicesSql, cancellationToken: ct);
        var devices = await connection.QueryAsync<DeviceRow>(command);

        return devices
            .Select(device => new DeviceSummaryDto(
                device.Id,
                device.Name,
                device.ProjectId,
                device.ProjectName,
                ToUtc(device.LastTelemetryAtUtc)))
            .ToList();
    }

    private static DateTime? ToUtc(DateTime? value) => value is null
        ? null
        : DateTime.SpecifyKind(value.Value, DateTimeKind.Utc);

    private sealed class DeviceRow
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public Guid ProjectId { get; init; }
        public string ProjectName { get; init; } = string.Empty;
        public DateTime? LastTelemetryAtUtc { get; init; }
    }
}
