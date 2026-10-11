namespace SmartLighting.Api.Application.Features.Dashboard.GetProjectSensorEvents;

public sealed record GetProjectSensorEventsResponse(
    int Page,
    int PageSize,
    int TotalCount,
    IReadOnlyCollection<GetProjectSensorEventsResponse.SensorEventResponse> Items)
{
    public sealed record SensorEventResponse(
        Guid EventIdentifier,
        Guid SensorId,
        Guid DeviceId,
        string SensorName,
        Guid ZoneId,
        string ZoneName,
        DateTime OccurredAtUtc);
}
