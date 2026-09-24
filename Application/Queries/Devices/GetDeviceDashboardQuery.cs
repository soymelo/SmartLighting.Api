using FastEndpoints;
using SmartLighting.Api.Application.Features.Devices.GetDeviceDashboard;

namespace SmartLighting.Api.Application.Queries.Devices;

public sealed class GetDeviceDashboardQuery(Guid deviceId) : ICommand<GetDeviceDashboardResponse?>
{
    public Guid DeviceId { get; } = deviceId;
}
