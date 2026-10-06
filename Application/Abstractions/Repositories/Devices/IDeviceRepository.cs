using SmartLighting.Api.Domain.Entities;

namespace SmartLighting.Api.Application.Abstractions.Repositories.Devices;

public interface IDeviceRepository
{
    Task<Device?> GetByIdWithComponentsAsync(Guid deviceId, CancellationToken ct);

    Task<bool> ProjectExistsAsync(Guid projectId, CancellationToken ct);

    Task<bool> DoZonesBelongToProjectAsync(
        Guid projectId,
        IReadOnlyCollection<Guid> zoneIds,
        CancellationToken ct);

    void Add(Device device);

    Task SaveChangesAsync(CancellationToken ct);
}
