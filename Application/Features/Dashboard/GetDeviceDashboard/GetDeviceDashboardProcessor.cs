using FastEndpoints;
using SmartLighting.Api.Application.Queries.Dashboard;

namespace SmartLighting.Api.Application.Features.Dashboard.GetDeviceDashboard;

public sealed class GetDeviceDashboardProcessor
{
    public Task<GetDeviceDashboardResponse?> HandleAsync(GetDeviceDashboardRequest request, CancellationToken ct)
    {
        var query = new GetDeviceDashboardQuery(request.DeviceId);

        return query.ExecuteAsync(ct);
    }
}
