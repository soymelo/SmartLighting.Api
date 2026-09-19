namespace SmartLighting.Api.Application.ReadModels.Devices;

public sealed record DeviceRecentSensorEventsDto(
    Guid DeviceId,
    IReadOnlyCollection<DeviceRecentSensorEventDto> Events);

public sealed record DeviceRecentSensorEventDto(
    Guid EventIdentifier,
    Guid SensorId,
    DateTime OccurredAtUtc);
