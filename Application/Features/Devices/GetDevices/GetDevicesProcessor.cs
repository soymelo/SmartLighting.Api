using FastEndpoints;
using SmartLighting.Api.Application.Queries.Devices;

namespace SmartLighting.Api.Application.Features.Devices.GetDevices;

public sealed class GetDevicesProcessor
{
    public Task<GetDevicesResponse> HandleAsync(CancellationToken ct)
    {
        var query = new GetDevicesQuery();

        return query.ExecuteAsync(ct);
    }
}
