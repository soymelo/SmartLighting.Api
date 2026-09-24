using FastEndpoints;
using SmartLighting.Api.Application.Queries.Devices;

namespace SmartLighting.Api.Application.Features.Devices.GetDeviceDashboard;

public sealed class GetDeviceDashboardProcessor
{
    public Task<GetDeviceDashboardResponse?> HandleAsync(GetDeviceDashboardRequest request, CancellationToken ct)
    {
        var query = new GetDeviceDashboardQuery(request.DeviceId);

        return query.ExecuteAsync(ct);
    }
}
