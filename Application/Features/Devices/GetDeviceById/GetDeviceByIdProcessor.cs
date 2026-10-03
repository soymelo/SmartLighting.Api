using FastEndpoints;
using SmartLighting.Api.Application.Queries.Devices;

namespace SmartLighting.Api.Application.Features.Devices.GetDeviceById;

public sealed class GetDeviceByIdProcessor
{
    public Task<GetDeviceByIdResponse?> HandleAsync(GetDeviceByIdRequest request, CancellationToken ct)
    {
        var query = new GetDeviceByIdQuery(request.DeviceId);

        return query.ExecuteAsync(ct);
    }
}
