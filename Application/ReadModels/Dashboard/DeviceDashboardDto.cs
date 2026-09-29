namespace SmartLighting.Api.Application.ReadModels.Dashboard;

public sealed record DeviceDashboardDto(
    Guid DeviceId,
    string Name,
    DateTime? LastTelemetryAtUtc,
    IReadOnlyCollection<DeviceDashboardLuminaireDto> Luminaires,
    IReadOnlyCollection<DeviceDashboardSensorEventDto> RecentSensorEvents);

public sealed record DeviceDashboardLuminaireDto(
    Guid LuminaireId,
    string Name,
    string ZoneName,
    int? BrightnessPercentage,
    DateTime? UpdatedAtUtc);

public sealed record DeviceDashboardSensorEventDto(
    Guid EventIdentifier,
    Guid SensorId,
    string SensorName,
    string ZoneName,
    DateTime OccurredAtUtc);
