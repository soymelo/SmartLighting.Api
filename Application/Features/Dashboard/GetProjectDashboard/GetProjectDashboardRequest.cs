using FastEndpoints;
using FluentValidation;

namespace SmartLighting.Api.Application.Features.Dashboard.GetProjectDashboard;

public sealed record GetProjectDashboardRequest(Guid ProjectId);

public sealed class GetProjectDashboardValidator : Validator<GetProjectDashboardRequest>
{
    public GetProjectDashboardValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
    }
}
