using Dapper;
using Microsoft.Data.SqlClient;
using SmartLighting.Api.Application.Abstractions.Queries.Dashboard;
using SmartLighting.Api.Application.ReadModels.Dashboard;

namespace SmartLighting.Api.Infrastructure.Queries.Dashboard;

public sealed class ProjectSensorEventsQueryRepository(IConfiguration configuration)
    : IProjectSensorEventsQueryRepository
{
    private readonly string _connectionString = configuration.GetConnectionString("SmartLightingDatabase")
        ?? throw new InvalidOperationException("A connection string 'SmartLightingDatabase' não foi configurada.");

    public async Task<ProjectSensorEventsPageDto?> GetPageAsync(
        Guid projectId, int page, int pageSize, string sortBy, bool descending, CancellationToken ct)
    {
        // Only these fixed expressions may enter ORDER BY. Values supplied by the client stay as SQL parameters.
        var sortColumn = sortBy switch
        {
            "sensorName" => "s.Name",
            "zoneName" => "z.Name",
            "deviceName" => "d.Name",
            _ => "se.OccurredAtUtc"
        };
        var direction = descending ? "DESC" : "ASC";
        var orderBy = sortColumn == "se.OccurredAtUtc"
            ? $"se.OccurredAtUtc {direction}, se.EventIdentifier {direction}"
            : $"{sortColumn} {direction}, se.OccurredAtUtc DESC, se.EventIdentifier DESC";

        var sql = $"""
            SELECT COUNT(*) FROM Projects WHERE Id = @ProjectId;

            SELECT COUNT(*)
            FROM SensorEvents AS se
            INNER JOIN Devices AS d ON d.Id = se.DeviceId
            INNER JOIN Sensors AS s ON s.Id = se.SensorId AND s.DeviceId = se.DeviceId
            INNER JOIN Zones AS z ON z.Id = s.ZoneId AND z.ProjectId = d.ProjectId
            WHERE d.ProjectId = @ProjectId;

            SELECT se.EventIdentifier,
                   se.SensorId,
                   se.DeviceId,
                   s.Name AS SensorName,
                   z.Id AS ZoneId,
                   z.Name AS ZoneName,
                   se.OccurredAtUtc
            FROM SensorEvents AS se
            INNER JOIN Devices AS d ON d.Id = se.DeviceId
            INNER JOIN Sensors AS s ON s.Id = se.SensorId AND s.DeviceId = se.DeviceId
            INNER JOIN Zones AS z ON z.Id = s.ZoneId AND z.ProjectId = d.ProjectId
            WHERE d.ProjectId = @ProjectId
            ORDER BY {orderBy}
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
            """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(ct);

        var command = new CommandDefinition(
            sql,
            new { ProjectId = projectId, Offset = ((long)page - 1) * pageSize, PageSize = pageSize },
            cancellationToken: ct);
        using var results = await connection.QueryMultipleAsync(command);

        if (await results.ReadSingleAsync<int>() == 0)
        {
            return null;
        }

        var totalCount = await results.ReadSingleAsync<int>();
        var items = (await results.ReadAsync<SensorEventRow>())
            .Select(item => new ProjectSensorEventDto(
                item.EventIdentifier,
                item.SensorId,
                item.DeviceId,
                item.SensorName,
                item.ZoneId,
                item.ZoneName,
                DateTime.SpecifyKind(item.OccurredAtUtc, DateTimeKind.Utc)))
            .ToList();

        return new ProjectSensorEventsPageDto(totalCount, items);
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
}
