using FastEndpoints;
using SmartLighting.Api.Application.Abstractions.Queries;
using SmartLighting.Api.Application.Features.Devices.GetRecentSensorEvents;

namespace SmartLighting.Api.Application.Queries.Devices;

public sealed class GetRecentSensorEventsHandler(IDeviceQueryRepository deviceQueryRepository)
    : ICommandHandler<GetRecentSensorEventsQuery, GetRecentSensorEventsResponse?>
{
    public async Task<GetRecentSensorEventsResponse?> ExecuteAsync(GetRecentSensorEventsQuery query, CancellationToken ct)
    {
        var sensorEvents = await deviceQueryRepository.GetRecentSensorEventsByDeviceIdAsync(
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
