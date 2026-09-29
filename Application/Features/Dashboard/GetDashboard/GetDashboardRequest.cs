using FastEndpoints;
using FluentValidation;

namespace SmartLighting.Api.Application.Features.Dashboard.GetDashboard;

public sealed record GetDashboardRequest(Guid DeviceId);

public sealed class GetDashboardValidator : Validator<GetDashboardRequest>
{
    public GetDashboardValidator()
    {
        RuleFor(x => x.DeviceId).NotEmpty();
    }
}
