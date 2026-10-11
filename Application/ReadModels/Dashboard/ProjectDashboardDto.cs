namespace SmartLighting.Api.Application.ReadModels.Dashboard;

public sealed record ProjectDashboardDto(
    Guid ProjectId,
    string Name,
    IReadOnlyCollection<ProjectDashboardDeviceDto> Devices,
    IReadOnlyCollection<ProjectDashboardZoneDto> Zones,
    IReadOnlyCollection<ProjectDashboardZoneActivityDto> ZoneActivity24h);

public sealed record ProjectDashboardZoneActivityDto(Guid ZoneId, int EventCount);

public sealed record ProjectDashboardDeviceDto(Guid DeviceId, string Name, DateTime? LastTelemetryAtUtc);

public sealed record ProjectDashboardZoneDto(
    Guid ZoneId,
    string Name,
    int DisplayOrder,
    IReadOnlyCollection<ProjectDashboardSensorDto> Sensors,
    IReadOnlyCollection<ProjectDashboardLuminaireDto> Luminaires);

public sealed record ProjectDashboardSensorDto(Guid SensorId, Guid DeviceId, string Name);

public sealed record ProjectDashboardLuminaireDto(
    Guid LuminaireId,
    Guid DeviceId,
    string Name,
    decimal NominalPowerWatts,
    int? BrightnessPercentage,
    DateTime? UpdatedAtUtc);
