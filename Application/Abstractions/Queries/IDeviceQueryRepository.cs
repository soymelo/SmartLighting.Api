using SmartLighting.Api.Application.ReadModels.Devices;

namespace SmartLighting.Api.Application.Abstractions.Queries;

public interface IDeviceQueryRepository
{
    Task<DeviceTopologyDto?> GetTopologyByIdAsync(Guid deviceId, CancellationToken ct);
}
