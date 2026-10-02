using SmartLighting.Api.Application.ReadModels.Projects;

namespace SmartLighting.Api.Application.Abstractions.Queries.Projects;

public interface IProjectQueryRepository
{
    Task<IReadOnlyCollection<ProjectSummaryDto>> GetAllAsync(CancellationToken ct);
}
