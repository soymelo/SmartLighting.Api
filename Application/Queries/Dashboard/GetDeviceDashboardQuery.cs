using FastEndpoints;
using SmartLighting.Api.Application.Features.Dashboard.GetDeviceDashboard;

namespace SmartLighting.Api.Application.Queries.Dashboard;

public sealed class GetDeviceDashboardQuery(Guid deviceId) : ICommand<GetDeviceDashboardResponse?>
{
    public Guid DeviceId { get; } = deviceId;
}
