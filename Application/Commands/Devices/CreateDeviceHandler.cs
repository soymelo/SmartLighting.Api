using FastEndpoints;
using SmartLighting.Api.Application.Abstractions.Repositories.Devices;
using SmartLighting.Api.Application.Features.Devices.CreateDevice;
using SmartLighting.Api.Domain.Entities;

namespace SmartLighting.Api.Application.Commands.Devices;

public sealed class CreateDeviceHandler(IDeviceRepository deviceRepository) : ICommandHandler<CreateDeviceCommand, CreateDeviceCommandResult>
{
    public async Task<CreateDeviceCommandResult> ExecuteAsync(CreateDeviceCommand command, CancellationToken ct)
    {
        if (!await deviceRepository.ProjectExistsAsync(command.ProjectId, ct))
        {
            return new CreateDeviceCommandResult(CreateDeviceCommandStatus.ProjectNotFound);
        }

        var zoneIds = command.Sensors
            .Select(sensor => sensor.ZoneId)
            .Concat(command.Luminaires.SelectMany(luminaire => luminaire.ZoneIds))
            .Distinct()
            .ToList();

        if (!await deviceRepository.DoZonesBelongToProjectAsync(command.ProjectId, zoneIds, ct))
        {
            return new CreateDeviceCommandResult(CreateDeviceCommandStatus.InvalidZone);
        }

        var device = new Device
        {
            Name = command.Name.Trim(),
            ProjectId = command.ProjectId
        };

        foreach (var sensor in command.Sensors)
        {
            device.Sensors.Add(new Sensor
            {
                DeviceId = device.Id,
                ZoneId = sensor.ZoneId,
                Name = sensor.Name.Trim(),
                Slot = sensor.Slot
            });
        }

        foreach (var luminaire in command.Luminaires)
        {
            var entity = new Luminaire
            {
                DeviceId = device.Id,
                Name = luminaire.Name.Trim(),
                Slot = luminaire.Slot,
                NominalPowerWatts = luminaire.NominalPowerWatts
            };

            foreach (var zoneId in luminaire.ZoneIds)
            {
                entity.ZoneLuminaires.Add(new ZoneLuminaire
                {
                    ZoneId = zoneId,
                    LuminaireId = entity.Id
                });
            }

            device.Luminaires.Add(entity);
        }

        deviceRepository.Add(device);
        await deviceRepository.SaveChangesAsync(ct);

        return new CreateDeviceCommandResult(
            CreateDeviceCommandStatus.Created,
            new CreateDeviceResponse(
                device.Id,
                device.Name,
                device.Sensors.Count,
                device.Luminaires.Count));
    }
}
