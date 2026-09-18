using FastEndpoints;
using SmartLighting.Api.Application.Abstractions.Queries;
using SmartLighting.Api.Application.Features.Devices.GetDeviceTopology;

namespace SmartLighting.Api.Application.Queries.Devices;

public sealed class GetDeviceTopologyHandler(IDeviceQueryRepository deviceQueryRepository)
    : ICommandHandler<GetDeviceTopologyQuery, GetDeviceTopologyResponse?>
{
    public async Task<GetDeviceTopologyResponse?> ExecuteAsync(GetDeviceTopologyQuery query, CancellationToken ct)
    {
        var device = await deviceQueryRepository.GetTopologyByIdAsync(query.DeviceId, ct);

        if (device is null)
        {
            return null;
        }

        var zones = device.Zones
            .Select(zone => new GetDeviceTopologyResponse.ZoneResponse(
                zone.ZoneId,
                zone.Name,
                zone.DisplayOrder,
                zone.Sensors
                    .Select(sensor => new GetDeviceTopologyResponse.SensorResponse(sensor.SensorId, sensor.Name))
                    .ToList(),
                zone.Luminaires
                    .Select(luminaire => new GetDeviceTopologyResponse.LuminaireResponse(
                        luminaire.LuminaireId,
                        luminaire.Name,
                        luminaire.NominalPowerWatts))
            .ToList()))
            .ToList();

        return new GetDeviceTopologyResponse(device.DeviceId, device.Name, zones);
    }
}
