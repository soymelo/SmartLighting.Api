using Dapper;
using Microsoft.Data.SqlClient;
using SmartLighting.Api.Application.Abstractions.Queries.Projects;
using SmartLighting.Api.Application.ReadModels.Projects;

namespace SmartLighting.Api.Infrastructure.Queries.Projects;

public sealed class ProjectQueryRepository(IConfiguration configuration) : IProjectQueryRepository
{
    private const string ProjectsSql = """
        SELECT p.Id, p.Name
        FROM Projects AS p
        ORDER BY p.Name, p.Id;
        """;

    private readonly string _connectionString = configuration.GetConnectionString("SmartLightingDatabase")
        ?? throw new InvalidOperationException("A connection string 'SmartLightingDatabase' não foi configurada.");

    public async Task<IReadOnlyCollection<ProjectSummaryDto>> GetAllAsync(CancellationToken ct)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(ct);

        var command = new CommandDefinition(ProjectsSql, cancellationToken: ct);
        var projects = await connection.QueryAsync<ProjectRow>(command);

        return projects
            .Select(project => new ProjectSummaryDto(project.Id, project.Name))
            .ToList();
    }

    private sealed class ProjectRow
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }
}
