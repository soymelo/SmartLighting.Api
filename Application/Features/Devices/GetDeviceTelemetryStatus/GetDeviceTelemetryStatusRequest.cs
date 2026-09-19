using FastEndpoints;
using FluentValidation;

namespace SmartLighting.Api.Application.Features.Devices.GetDeviceTelemetryStatus;

public sealed record GetDeviceTelemetryStatusRequest(Guid DeviceId);

public sealed class GetDeviceTelemetryStatusValidator : Validator<GetDeviceTelemetryStatusRequest>
{
    public GetDeviceTelemetryStatusValidator()
    {
        RuleFor(x => x.DeviceId).NotEmpty();
    }
}
