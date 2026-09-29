using SmartLighting.Api.Application.ReadModels.Devices;

namespace SmartLighting.Api.Application.Abstractions.Queries.Devices;

public interface IDeviceQueryRepository
{
    Task<IReadOnlyCollection<DeviceSummaryDto>> GetAllAsync(CancellationToken ct);
}
