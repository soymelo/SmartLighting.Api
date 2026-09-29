using FastEndpoints;
using SmartLighting.Api.Application.Queries.Dashboard;

namespace SmartLighting.Api.Application.Features.Dashboard.GetDashboard;

public sealed class GetDashboardProcessor
{
    public Task<GetDashboardResponse?> HandleAsync(GetDashboardRequest request, CancellationToken ct)
    {
        var query = new GetDashboardQuery(request.DeviceId);

        return query.ExecuteAsync(ct);
    }
}
