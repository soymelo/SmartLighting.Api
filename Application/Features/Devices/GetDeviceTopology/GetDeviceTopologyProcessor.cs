using FastEndpoints;
using SmartLighting.Api.Application.Queries.Devices;

namespace SmartLighting.Api.Application.Features.Devices.GetDeviceTopology;

public sealed class GetDeviceTopologyProcessor
{
    public Task<GetDeviceTopologyResponse?> HandleAsync(GetDeviceTopologyRequest req, CancellationToken ct)
    {
        var query = new GetDeviceTopologyQuery(req.DeviceId);

        return query.ExecuteAsync(ct);
    }
}
