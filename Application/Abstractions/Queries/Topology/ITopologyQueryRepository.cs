using SmartLighting.Api.Application.ReadModels.Topology;

namespace SmartLighting.Api.Application.Abstractions.Queries.Topology;

public interface ITopologyQueryRepository
{
    Task<DeviceTopologyDto?> GetTopologyByIdAsync(Guid deviceId, CancellationToken ct);
    Task<ProjectControlTopologyDto?> GetProjectControlTopologyByDeviceIdAsync(Guid deviceId, CancellationToken ct);
}
