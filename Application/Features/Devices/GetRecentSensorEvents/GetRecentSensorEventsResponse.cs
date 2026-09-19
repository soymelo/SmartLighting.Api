namespace SmartLighting.Api.Application.Features.Devices.GetRecentSensorEvents;

public sealed record GetRecentSensorEventsResponse(
    Guid DeviceId,
    IReadOnlyCollection<GetRecentSensorEventsResponse.SensorEventResponse> Events)
{
    public sealed record SensorEventResponse(
        Guid EventIdentifier,
        Guid SensorId,
        DateTime OccurredAtUtc);
}
