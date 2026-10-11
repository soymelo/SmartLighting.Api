using FastEndpoints;
using SmartLighting.Api.Application.Queries.Dashboard;

namespace SmartLighting.Api.Application.Features.Dashboard.GetProjectSensorEvents;

public sealed class GetProjectSensorEventsProcessor
{
    public Task<GetProjectSensorEventsResponse?> HandleAsync(GetProjectSensorEventsRequest request, CancellationToken ct)
    {
        var query = new GetProjectSensorEventsQuery(
            request.ProjectId, request.Page, request.PageSize, request.SortBy, request.Descending);

        return query.ExecuteAsync(ct);
    }
}
