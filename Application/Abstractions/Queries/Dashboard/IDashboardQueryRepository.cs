using SmartLighting.Api.Application.ReadModels.Dashboard;

namespace SmartLighting.Api.Application.Abstractions.Queries.Dashboard;

public interface IDashboardQueryRepository
{
    Task<DeviceDashboardDto?> GetDashboardByIdAsync(Guid deviceId, CancellationToken ct);
}
