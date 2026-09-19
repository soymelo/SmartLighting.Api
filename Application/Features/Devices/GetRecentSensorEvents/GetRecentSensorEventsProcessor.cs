using FastEndpoints;
using SmartLighting.Api.Application.Queries.Devices;

namespace SmartLighting.Api.Application.Features.Devices.GetRecentSensorEvents;

public sealed class GetRecentSensorEventsProcessor
{
    public Task<GetRecentSensorEventsResponse?> HandleAsync(GetRecentSensorEventsRequest request, CancellationToken ct)
    {
        var query = new GetRecentSensorEventsQuery(request.DeviceId, request.Limit);

        return query.ExecuteAsync(ct);
    }
}
