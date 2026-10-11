namespace SmartLighting.Api.Application.ReadModels.Dashboard;

public sealed record ProjectSensorEventsPageDto(
    int TotalCount,
    IReadOnlyCollection<ProjectSensorEventDto> Items);

public sealed record ProjectSensorEventDto(
    Guid EventIdentifier,
    Guid SensorId,
    Guid DeviceId,
    string SensorName,
    Guid ZoneId,
    string ZoneName,
    DateTime OccurredAtUtc);
