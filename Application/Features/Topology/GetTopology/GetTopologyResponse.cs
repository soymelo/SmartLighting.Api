namespace SmartLighting.Api.Application.Features.Topology.GetTopology;

public sealed record GetTopologyResponse
(
    Guid DeviceId,
    string Name,
    IReadOnlyCollection<GetTopologyResponse.ZoneResponse> Zones
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

    public sealed record SensorResponse(
        Guid SensorId,
        Guid DeviceId,
        string Name,
        int Slot,
        int GpioNumber);

    public sealed record LuminaireResponse
    (
        Guid LuminaireId,
        Guid DeviceId,
        string Name,
        decimal NominalPowerWatts,
        int Slot,
        int GpioNumber,
        IReadOnlyCollection<Guid> ZoneIds
    );
}
