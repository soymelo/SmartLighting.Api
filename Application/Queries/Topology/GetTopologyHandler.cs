using FastEndpoints;
using SmartLighting.Api.Application.Abstractions.Queries.Topology;
using SmartLighting.Api.Application.Features.Topology.GetTopology;

namespace SmartLighting.Api.Application.Queries.Topology;

public sealed class GetTopologyHandler(ITopologyQueryRepository topologyQueryRepository)
    : ICommandHandler<GetTopologyQuery, GetTopologyResponse?>
{
    public async Task<GetTopologyResponse?> ExecuteAsync(GetTopologyQuery query, CancellationToken ct)
    {
        var device = await topologyQueryRepository.GetProjectControlTopologyByDeviceIdAsync(query.DeviceId, ct);

        if (device is null)
        {
            return null;
        }

        var zoneIdsByLuminaire = device.Zones
            .SelectMany(zone => zone.Luminaires.Select(luminaire => (zone.ZoneId, luminaire.LuminaireId)))
            .GroupBy(item => item.LuminaireId)
            .ToDictionary(group => group.Key, group => (IReadOnlyCollection<Guid>)group
                .Select(item => item.ZoneId)
                .ToArray());

        var zones = device.Zones
            .Select(zone => new GetTopologyResponse.ZoneResponse(
                zone.ZoneId,
                zone.Name,
                zone.DisplayOrder,
                zone.Sensors
                    .Select(sensor => new GetTopologyResponse.SensorResponse(
                        sensor.SensorId,
                        sensor.DeviceId,
                        sensor.Name,
                        (int)sensor.Slot,
                        sensor.GpioNumber))
                    .ToList(),
                zone.Luminaires
                    .Select(luminaire => new GetTopologyResponse.LuminaireResponse(
                        luminaire.LuminaireId,
                        luminaire.DeviceId,
                        luminaire.Name,
                        luminaire.NominalPowerWatts,
                        (int)luminaire.Slot,
                        luminaire.GpioNumber,
                        zoneIdsByLuminaire[luminaire.LuminaireId]))
            .ToList()))
            .ToList();

        return new GetTopologyResponse(device.DeviceId, device.Name, zones);
    }
}
