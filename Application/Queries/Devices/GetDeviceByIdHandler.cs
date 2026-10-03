using FastEndpoints;
using SmartLighting.Api.Application.Abstractions.Queries.Topology;
using SmartLighting.Api.Application.Features.Devices.GetDeviceById;

namespace SmartLighting.Api.Application.Queries.Devices;

public sealed class GetDeviceByIdHandler(ITopologyQueryRepository topologyQueryRepository)
    : ICommandHandler<GetDeviceByIdQuery, GetDeviceByIdResponse?>
{
    public async Task<GetDeviceByIdResponse?> ExecuteAsync(GetDeviceByIdQuery query, CancellationToken ct)
    {
        var device = await topologyQueryRepository.GetTopologyByIdAsync(query.DeviceId, ct);

        if (device is null)
        {
            return null;
        }

        var zones = device.Zones
            .Select(zone => new GetDeviceByIdResponse.ZoneResponse(
                zone.ZoneId,
                zone.Name,
                zone.DisplayOrder,
                zone.Sensors
                    .Select(sensor => new GetDeviceByIdResponse.SensorResponse(
                        sensor.SensorId,
                        sensor.Name,
                        (int)sensor.Slot,
                        sensor.GpioNumber))
                    .ToList(),
                zone.Luminaires
                    .Select(luminaire => new GetDeviceByIdResponse.LuminaireResponse(
                        luminaire.LuminaireId,
                        luminaire.Name,
                        luminaire.NominalPowerWatts,
                        (int)luminaire.Slot,
                        luminaire.GpioNumber))
                    .ToList()))
            .ToList();

        return new GetDeviceByIdResponse(
            device.DeviceId,
            device.Name,
            device.ProjectId,
            device.ProjectName,
            device.LastTelemetryAtUtc,
            zones);
    }
}
