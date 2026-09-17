namespace SmartLighting.Api.Application.Features.Devices.GetDeviceTopology;

public sealed record GetDeviceTopologyResponse
(
    string DeviceIdentifier,
    IReadOnlyCollection<GetDeviceTopologyResponse.ZoneResponse> Zones
)
{
    public sealed record ZoneResponse
    (
        string ZoneIdentifier,
        int DisplayOrder,
        IReadOnlyCollection<SensorResponse> Sensors,
        IReadOnlyCollection<LuminaireResponse> Luminaires
    );

    public sealed record SensorResponse(string SensorIdentifier);

    public sealed record LuminaireResponse
    (
        string LuminaireIdentifier,
        decimal NominalPowerWatts
    );
}
