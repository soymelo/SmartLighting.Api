namespace SmartLighting.Api.Application.ReadModels.Devices;

public sealed record DeviceTopologyDto(
    string DeviceIdentifier,
    IReadOnlyCollection<DeviceTopologyZoneDto> Zones);

public sealed record DeviceTopologyZoneDto(
    string ZoneIdentifier,
    int DisplayOrder,
    IReadOnlyCollection<DeviceTopologySensorDto> Sensors,
    IReadOnlyCollection<DeviceTopologyLuminaireDto> Luminaires);

public sealed record DeviceTopologySensorDto(string SensorIdentifier);

public sealed record DeviceTopologyLuminaireDto(
    string LuminaireIdentifier,
    decimal NominalPowerWatts);
