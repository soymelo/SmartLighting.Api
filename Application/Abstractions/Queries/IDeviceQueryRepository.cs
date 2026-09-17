using SmartLighting.Api.Application.ReadModels.Devices;

namespace SmartLighting.Api.Application.Abstractions.Queries;

public interface IDeviceQueryRepository
{
    Task<DeviceTopologyDto?> GetTopologyByIdentifierAsync(
        string deviceIdentifier,
        CancellationToken ct);
}
