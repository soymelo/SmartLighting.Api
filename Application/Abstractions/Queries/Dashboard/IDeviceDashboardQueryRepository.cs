using SmartLighting.Api.Application.ReadModels.Dashboard;

namespace SmartLighting.Api.Application.Abstractions.Queries.Dashboard;

public interface IDeviceDashboardQueryRepository
{
    Task<DeviceDashboardDto?> GetDeviceDashboardByIdAsync(Guid deviceId, CancellationToken ct);
}
