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
            .Include(d => d.Zones)
            .ThenInclude(z => z.Luminaires)
            .Include(d => d.Sensors)
            .SingleOrDefaultAsync(d => d.DeviceIdentifier == deviceIdentifier, ct);

        if (device is null)
            return null;

        var zones = device.Zones
            .OrderBy(z => z.DisplayOrder)
            .Select(z => new ZoneTopologyResponse(z.ZoneIdentifier,z.DisplayOrder,
                device.Sensors
                      .Where(s => s.ZoneId == z.Id)
                      .OrderBy(s => s.SensorIdentifier)
                      .Select(s => new SensorTopologyResponse(s.SensorIdentifier))
                      .ToList(),
                z.Luminaires
                    .OrderBy(l => l.LuminaireIdentifier)
                    .Select(l => new LuminaireTopologyResponse(
                        l.LuminaireIdentifier,
                        l.NominalPowerWatts))
                    .ToList()))
            .ToList();

        return new GetDeviceTopologyResponse(device.DeviceIdentifier, zones);
    }
}
