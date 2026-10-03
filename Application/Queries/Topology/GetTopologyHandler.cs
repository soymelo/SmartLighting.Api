using FastEndpoints;
using SmartLighting.Api.Application.Abstractions.Queries.Topology;
using SmartLighting.Api.Application.Features.Topology.GetTopology;

namespace SmartLighting.Api.Application.Queries.Topology;

public sealed class GetTopologyHandler(ITopologyQueryRepository topologyQueryRepository)
    : ICommandHandler<GetTopologyQuery, GetTopologyResponse?>
{
    public async Task<GetTopologyResponse?> ExecuteAsync(GetTopologyQuery query, CancellationToken ct)
    {
        var device = await topologyQueryRepository.GetTopologyByIdAsync(query.DeviceId, ct);

        if (device is null)
        {
            return null;
        }

        var zones = device.Zones
            .Select(zone => new GetTopologyResponse.ZoneResponse(
                zone.ZoneId,
                zone.Name,
                zone.DisplayOrder,
                zone.Sensors
                    .Select(sensor => new GetTopologyResponse.SensorResponse(
                        sensor.SensorId,
                        sensor.Name,
                        (int)sensor.Slot,
                        sensor.GpioNumber))
                    .ToList(),
                zone.Luminaires
                    .Select(luminaire => new GetTopologyResponse.LuminaireResponse(
                        luminaire.LuminaireId,
                        luminaire.Name,
                        luminaire.NominalPowerWatts,
                        (int)luminaire.Slot,
                        luminaire.GpioNumber))
            .ToList()))
            .ToList();

        return new GetTopologyResponse(device.DeviceId, device.Name, zones);
    }
}
