namespace SmartLighting.Api.Application.ReadModels.Topology;

public sealed record DeviceTopologyDto(
    Guid DeviceId,
    string Name,
    Guid ProjectId,
    string ProjectName,
    DateTime? LastTelemetryAtUtc,
    IReadOnlyCollection<DeviceTopologyZoneDto> Zones);

public sealed record DeviceTopologyZoneDto(
    Guid ZoneId,
    string Name,
    int DisplayOrder,
    IReadOnlyCollection<DeviceTopologySensorDto> Sensors,
    IReadOnlyCollection<DeviceTopologyLuminaireDto> Luminaires);

public sealed record DeviceTopologySensorDto(Guid SensorId, string Name);

public sealed record DeviceTopologyLuminaireDto(
    Guid LuminaireId,
    string Name,
    decimal NominalPowerWatts);
