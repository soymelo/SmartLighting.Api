using FastEndpoints;
using FluentValidation;

namespace SmartLighting.Api.Application.Features.Dashboard.GetDeviceDashboard;

public sealed record GetDeviceDashboardRequest(Guid DeviceId);

public sealed class GetDeviceDashboardValidator : Validator<GetDeviceDashboardRequest>
{
    public GetDeviceDashboardValidator()
    {
        RuleFor(x => x.DeviceId).NotEmpty();
    }
}
