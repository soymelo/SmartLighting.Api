using FastEndpoints;
using SmartLighting.Api.Application.Abstractions.Repositories.Devices;
using SmartLighting.Api.Application.Features.Devices.UpdateDevice;
using SmartLighting.Api.Domain.Entities;

namespace SmartLighting.Api.Application.Commands.Devices;

public sealed class UpdateDeviceHandler(IDeviceRepository deviceRepository)
    : ICommandHandler<UpdateDeviceCommand, UpdateDeviceResponse>
{
    public async Task<UpdateDeviceResponse> ExecuteAsync(UpdateDeviceCommand command, CancellationToken ct)
    {
        var device = await deviceRepository.GetByIdWithComponentsAsync(command.DeviceId, ct);

        if (device is null)
        {
            return new UpdateDeviceNotFoundResponse();
        }

        var sensorSlots = command.Sensors.Select(sensor => sensor.Slot).ToHashSet();
        var luminaireSlots = command.Luminaires.Select(luminaire => luminaire.Slot).ToHashSet();

        if (device.Sensors.Any(sensor => !sensorSlots.Contains(sensor.Slot)) ||
            device.Luminaires.Any(luminaire => !luminaireSlots.Contains(luminaire.Slot)))
        {
            return new UpdateDeviceMissingSlotsResponse();
        }

        var zoneIds = command.Sensors
            .Select(sensor => sensor.ZoneId)
            .Concat(command.Luminaires.SelectMany(luminaire => luminaire.ZoneIds))
            .Distinct()
            .ToList();

        if (!await deviceRepository.DoZonesBelongToProjectAsync(device.ProjectId, zoneIds, ct))
        {
            return new UpdateDeviceInvalidZoneResponse();
        }

        device.Name = command.Name.Trim();

        foreach (var item in command.Sensors)
        {
            var sensor = device.Sensors.SingleOrDefault(sensor => sensor.Slot == item.Slot);

            if (sensor is null)
            {
                device.Sensors.Add(new Sensor
                {
                    DeviceId = device.Id,
                    Slot = item.Slot,
                    Name = item.Name.Trim(),
                    ZoneId = item.ZoneId
                });
                continue;
            }

            sensor.Name = item.Name.Trim();
            sensor.ZoneId = item.ZoneId;
        }

        foreach (var item in command.Luminaires)
        {
            var luminaire = device.Luminaires.SingleOrDefault(luminaire => luminaire.Slot == item.Slot);

            if (luminaire is null)
            {
                luminaire = new Luminaire
                {
                    DeviceId = device.Id,
                    Slot = item.Slot,
                    Name = item.Name.Trim(),
                    NominalPowerWatts = item.NominalPowerWatts
                };
                device.Luminaires.Add(luminaire);
            }
            else
            {
                luminaire.Name = item.Name.Trim();
                luminaire.NominalPowerWatts = item.NominalPowerWatts;
            }

            var requestedZoneIds = item.ZoneIds.ToHashSet();
            foreach (var link in luminaire.ZoneLuminaires
                         .Where(link => !requestedZoneIds.Contains(link.ZoneId))
                         .ToList())
            {
                luminaire.ZoneLuminaires.Remove(link);
            }

            var existingZoneIds = luminaire.ZoneLuminaires
                .Select(link => link.ZoneId)
                .ToHashSet();
            foreach (var zoneId in requestedZoneIds.Except(existingZoneIds))
            {
                luminaire.ZoneLuminaires.Add(new ZoneLuminaire
                {
                    ZoneId = zoneId,
                    LuminaireId = luminaire.Id
                });
            }
        }

        await deviceRepository.SaveChangesAsync(ct);

        return new DeviceUpdatedResponse(
            device.Id,
            device.Name,
            device.Sensors.Count,
            device.Luminaires.Count);
    }
}
