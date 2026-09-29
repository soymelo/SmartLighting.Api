using FastEndpoints;
using SmartLighting.Api.Application.Queries.Telemetry;

namespace SmartLighting.Api.Application.Features.Telemetry.GetRecentSensorEvents;

public sealed class GetRecentSensorEventsProcessor
{
    public Task<GetRecentSensorEventsResponse?> HandleAsync(GetRecentSensorEventsRequest request, CancellationToken ct)
    {
        var query = new GetRecentSensorEventsQuery(request.DeviceId, request.Limit);

        return query.ExecuteAsync(ct);
    }
}
