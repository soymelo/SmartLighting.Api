using SmartLighting.Api.Domain.Hardware;

namespace SmartLighting.Api.Application.ReadModels.Topology;

public sealed record ProjectControlTopologyDto(
    Guid DeviceId,
    string Name,
    IReadOnlyCollection<ProjectControlZoneDto> Zones);

public sealed record ProjectControlZoneDto(
    Guid ZoneId,
    string Name,
    int DisplayOrder,
    IReadOnlyCollection<ProjectControlSensorDto> Sensors,
    IReadOnlyCollection<ProjectControlLuminaireDto> Luminaires);

public sealed record ProjectControlSensorDto(
    Guid SensorId,
    Guid DeviceId,
    string Name,
    SensorSlot Slot,
    int GpioNumber);

public sealed record ProjectControlLuminaireDto(
    Guid LuminaireId,
    Guid DeviceId,
    string Name,
    decimal NominalPowerWatts,
    LuminaireSlot Slot,
    int GpioNumber);
