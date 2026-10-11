using FastEndpoints;
using SmartLighting.Api.Application.Features.Dashboard.GetProjectSensorEvents;

namespace SmartLighting.Api.Application.Queries.Dashboard;

public sealed class GetProjectSensorEventsQuery(
    Guid projectId, int page, int pageSize, string sortBy, bool descending)
    : ICommand<GetProjectSensorEventsResponse?>
{
    public Guid ProjectId { get; } = projectId;
    public int Page { get; } = page;
    public int PageSize { get; } = pageSize;
    public string SortBy { get; } = sortBy;
    public bool Descending { get; } = descending;
}
