namespace SmartLighting.Api.Application.Features.Devices.GetDeviceTopology;

public sealed record GetDeviceTopologyResponse
(
    Guid DeviceId,
    string Name,
    IReadOnlyCollection<GetDeviceTopologyResponse.ZoneResponse> Zones
)
{
    public sealed record ZoneResponse
    (
        Guid ZoneId,
        string Name,
        int DisplayOrder,
        IReadOnlyCollection<SensorResponse> Sensors,
        IReadOnlyCollection<LuminaireResponse> Luminaires
    );

    public sealed record SensorResponse(Guid SensorId, string Name);

    public sealed record LuminaireResponse
    (
        Guid LuminaireId,
        string Name,
        decimal NominalPowerWatts
    );
}
