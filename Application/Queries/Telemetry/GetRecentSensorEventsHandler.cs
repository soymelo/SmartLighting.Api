using FastEndpoints;
using SmartLighting.Api.Application.Abstractions.Queries.Telemetry;
using SmartLighting.Api.Application.Features.Telemetry.GetRecentSensorEvents;

namespace SmartLighting.Api.Application.Queries.Telemetry;

public sealed class GetRecentSensorEventsHandler(ITelemetryQueryRepository telemetryQueryRepository)
    : ICommandHandler<GetRecentSensorEventsQuery, GetRecentSensorEventsResponse?>
{
    public async Task<GetRecentSensorEventsResponse?> ExecuteAsync(GetRecentSensorEventsQuery query, CancellationToken ct)
    {
        var sensorEvents = await telemetryQueryRepository.GetRecentSensorEventsByDeviceIdAsync(
            query.DeviceId,
            query.Limit,
            ct);

        if (sensorEvents is null)
        {
            return null;
        }

        var events = sensorEvents.Events
            .Select(sensorEvent => new GetRecentSensorEventsResponse.SensorEventResponse(
                sensorEvent.EventIdentifier,
                sensorEvent.SensorId,
                sensorEvent.OccurredAtUtc)).ToList();

        return new GetRecentSensorEventsResponse(sensorEvents.DeviceId, events);
    }
}
