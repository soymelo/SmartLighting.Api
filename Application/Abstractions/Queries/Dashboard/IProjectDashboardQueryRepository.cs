using SmartLighting.Api.Application.ReadModels.Dashboard;

namespace SmartLighting.Api.Application.Abstractions.Queries.Dashboard;

public interface IProjectDashboardQueryRepository
{
    Task<ProjectDashboardDto?> GetProjectDashboardByIdAsync(Guid projectId, CancellationToken ct);
}
