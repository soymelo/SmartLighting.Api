using FastEndpoints;
using SmartLighting.Api.Application.Features.Devices.GetDeviceTopology;

namespace SmartLighting.Api.Application.Queries.Devices;

public sealed class GetDeviceTopologyQuery(string deviceIdentifier) : ICommand<GetDeviceTopologyResponse?>
{
    public string DeviceIdentifier { get; } = deviceIdentifier;
}
