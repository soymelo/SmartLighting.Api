using FastEndpoints;
using FluentValidation;

namespace SmartLighting.Api.Application.Features.Telemetry.GetTelemetryStatus;

public sealed record GetTelemetryStatusRequest(Guid DeviceId);

public sealed class GetTelemetryStatusValidator : Validator<GetTelemetryStatusRequest>
{
    public GetTelemetryStatusValidator()
    {
        RuleFor(x => x.DeviceId).NotEmpty();
    }
}
