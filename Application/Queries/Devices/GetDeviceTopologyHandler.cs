using FastEndpoints;
using SmartLighting.Api.Application.Abstractions.Queries;
using SmartLighting.Api.Application.Features.Devices.GetDeviceTopology;

namespace SmartLighting.Api.Application.Queries.Devices;

public sealed class GetDeviceTopologyHandler(IDeviceQueryRepository deviceQueryRepository)
    : ICommandHandler<GetDeviceTopologyQuery, GetDeviceTopologyResponse?>
{
    public async Task<GetDeviceTopologyResponse?> ExecuteAsync(GetDeviceTopologyQuery query, CancellationToken ct)
    {
        var device = await deviceQueryRepository.GetTopologyByIdentifierAsync(query.DeviceIdentifier, ct);

        if (device is null)
        {
            return null;
        }

        var zones = device.Zones
            .Select(zone => new GetDeviceTopologyResponse.ZoneResponse(
                zone.ZoneIdentifier,
                zone.DisplayOrder,
                zone.Sensors
                    .Select(sensor => new GetDeviceTopologyResponse.SensorResponse(sensor.SensorIdentifier))
                    .ToList(),
                zone.Luminaires
                    .Select(luminaire => new GetDeviceTopologyResponse.LuminaireResponse(
                        luminaire.LuminaireIdentifier,
                        luminaire.NominalPowerWatts))
                    .ToList()))
            .ToList();

        return new GetDeviceTopologyResponse(device.DeviceIdentifier, zones);
    }
}
