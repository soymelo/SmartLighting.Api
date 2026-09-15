using FastEndpoints;
using SmartLighting.Api.Application.Abstractions.Repositories;
using SmartLighting.Api.Application.Features.Devices.GetDeviceTopology;

namespace SmartLighting.Api.Application.Queries.Devices;

public sealed class GetDeviceTopologyHandler(IDeviceRepository deviceRepository)
    : ICommandHandler<GetDeviceTopologyQuery, GetDeviceTopologyResponse?>
{
    public Task<GetDeviceTopologyResponse?> ExecuteAsync(GetDeviceTopologyQuery query, CancellationToken ct)
    {
        return deviceRepository.GetTopologyByIdentifierAsync(query.DeviceIdentifier, ct);
    }
}
