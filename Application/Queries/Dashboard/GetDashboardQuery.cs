using FastEndpoints;
using SmartLighting.Api.Application.Features.Dashboard.GetDashboard;

namespace SmartLighting.Api.Application.Queries.Dashboard;

public sealed class GetDashboardQuery(Guid deviceId) : ICommand<GetDashboardResponse?>
{
    public Guid DeviceId { get; } = deviceId;
}
