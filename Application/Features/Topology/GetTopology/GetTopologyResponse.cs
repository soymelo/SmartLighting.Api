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

    public sealed record SensorResponse(Guid SensorId, string Name);

    public sealed record LuminaireResponse
    (
        Guid LuminaireId,
        string Name,
        decimal NominalPowerWatts
    );
}
