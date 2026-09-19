using FastEndpoints;
using SmartLighting.Api.Application.Queries.Devices;

namespace SmartLighting.Api.Application.Features.Devices.GetDeviceTelemetryStatus;

public sealed class GetDeviceTelemetryStatusProcessor
{
    public Task<GetDeviceTelemetryStatusResponse?> HandleAsync(GetDeviceTelemetryStatusRequest request,CancellationToken ct)
    {
        var query = new GetDeviceTelemetryStatusQuery(request.DeviceId);

        return query.ExecuteAsync(ct);
    }
}
