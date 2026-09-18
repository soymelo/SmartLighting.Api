using FastEndpoints;
using SmartLighting.Api.Application.Features.Devices.GetDeviceTopology;

namespace SmartLighting.Api.Application.Queries.Devices;

public sealed class GetDeviceTopologyQuery(Guid deviceId) : ICommand<GetDeviceTopologyResponse?>
{
    public Guid DeviceId { get; } = deviceId;
}
