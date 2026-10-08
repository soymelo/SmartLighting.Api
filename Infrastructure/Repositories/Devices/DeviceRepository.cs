using Microsoft.EntityFrameworkCore;
using SmartLighting.Api.Application.Abstractions.Repositories.Devices;
using SmartLighting.Api.Domain.Entities;
using SmartLighting.Api.Infrastructure.Persistence;

namespace SmartLighting.Api.Infrastructure.Repositories.Devices;

public sealed class DeviceRepository(SmartLightingDbContext dbContext) : IDeviceRepository
{
    public Task<Device?> GetByIdWithComponentsAsync(Guid deviceId, CancellationToken ct) =>
        dbContext.Devices
            .Include(device => device.Sensors)
            .Include(device => device.Luminaires)
                .ThenInclude(luminaire => luminaire.ZoneLuminaires)
            .AsSplitQuery()
            .SingleOrDefaultAsync(device => device.Id == deviceId, ct);

    public Task<bool> ProjectExistsAsync(Guid projectId, CancellationToken ct) =>
        dbContext.Projects.AnyAsync(project => project.Id == projectId, ct);

    public async Task<bool> DoZonesBelongToProjectAsync(
        Guid projectId,
        IReadOnlyCollection<Guid> zoneIds,
        CancellationToken ct)
    {
        var distinctZoneIds = zoneIds.Distinct().ToList();

        if (distinctZoneIds.Count == 0)
        {
            return true;
        }

        var zonesInProjectCount = await dbContext.Zones.CountAsync(
            zone => distinctZoneIds.Contains(zone.Id) && zone.ProjectId == projectId,
            ct);

        return zonesInProjectCount == distinctZoneIds.Count;
    }

    public void Add(Device device) =>
        dbContext.Devices.Add(device);

    public Task SaveChangesAsync(CancellationToken ct) =>
        dbContext.SaveChangesAsync(ct);
}
