using SmartLighting.Api.Application.ReadModels.Dashboard;

namespace SmartLighting.Api.Application.Abstractions.Queries.Dashboard;

public interface IProjectSensorEventsQueryRepository
{
    Task<ProjectSensorEventsPageDto?> GetPageAsync(
        Guid projectId, int page, int pageSize, string sortBy, bool descending, CancellationToken ct);
}
