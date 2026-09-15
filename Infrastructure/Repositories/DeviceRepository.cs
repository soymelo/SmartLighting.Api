using Microsoft.EntityFrameworkCore;
using SmartLighting.Api.Application.Abstractions.Repositories;
using SmartLighting.Api.Application.Features.Devices.GetDeviceTopology;
using SmartLighting.Api.Infrastructure.Persistence;

namespace SmartLighting.Api.Infrastructure.Repositories;

public sealed class DeviceRepository(SmartLightingDbContext dbContext) : IDeviceRepository
{
    public async Task<GetDeviceTopologyResponse?> GetTopologyByIdentifierAsync(string deviceIdentifier,CancellationToken ct)
    {
        var device = await dbContext.Devices
            .AsNoTracking()
            .AsSplitQuery()
            .Include(device => device.Zones)
            .ThenInclude(zone => zone.Luminaires)
            .Include(device => device.Sensors)
            .SingleOrDefaultAsync(
                device => device.DeviceIdentifier == deviceIdentifier,
                ct);

        if (device is null)
        {
            return null;
        }

        var zones = device.Zones
            .OrderBy(zone => zone.DisplayOrder)
            .Select(zone => new ZoneTopologyResponse(
                zone.ZoneIdentifier,
                zone.DisplayOrder,
                device.Sensors
                    .Where(sensor => sensor.ZoneId == zone.Id)
                    .OrderBy(sensor => sensor.SensorIdentifier)
                    .Select(sensor => new SensorTopologyResponse(sensor.SensorIdentifier))
                    .ToList(),
                zone.Luminaires
                    .OrderBy(luminaire => luminaire.LuminaireIdentifier)
                    .Select(luminaire => new LuminaireTopologyResponse(
                        luminaire.LuminaireIdentifier,
                        luminaire.NominalPowerWatts))
                    .ToList()))
            .ToList();

        return new GetDeviceTopologyResponse(device.DeviceIdentifier, zones);
    }
}
