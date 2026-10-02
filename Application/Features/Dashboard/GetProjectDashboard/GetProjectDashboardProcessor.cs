using FastEndpoints;
using SmartLighting.Api.Application.Queries.Dashboard;

namespace SmartLighting.Api.Application.Features.Dashboard.GetProjectDashboard;

public sealed class GetProjectDashboardProcessor
{
    public Task<GetProjectDashboardResponse?> HandleAsync(GetProjectDashboardRequest request, CancellationToken ct)
    {
        var query = new GetProjectDashboardQuery(request.ProjectId);

        return query.ExecuteAsync(ct);
    }
}
