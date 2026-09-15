namespace SmartLighting.Api.Application.Features.Devices.GetDeviceTopology;

public sealed record GetDeviceTopologyResponse(
    string DeviceIdentifier,
    IReadOnlyCollection<ZoneTopologyResponse> Zones);

public sealed record ZoneTopologyResponse(
    string ZoneIdentifier,
    int DisplayOrder,
    IReadOnlyCollection<SensorTopologyResponse> Sensors,
    IReadOnlyCollection<LuminaireTopologyResponse> Luminaires);

public sealed record SensorTopologyResponse(string SensorIdentifier);

public sealed record LuminaireTopologyResponse(
    string LuminaireIdentifier,
    decimal NominalPowerWatts);
