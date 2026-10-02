using FastEndpoints;
using SmartLighting.Api.Application.Features.Dashboard.GetProjectDashboard;

namespace SmartLighting.Api.Application.Queries.Dashboard;

public sealed class GetProjectDashboardQuery(Guid projectId) : ICommand<GetProjectDashboardResponse?>
{
    public Guid ProjectId { get; } = projectId;
}
