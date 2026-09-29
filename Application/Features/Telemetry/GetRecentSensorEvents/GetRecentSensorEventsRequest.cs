using FastEndpoints;
using FluentValidation;

namespace SmartLighting.Api.Application.Features.Telemetry.GetRecentSensorEvents;

public sealed record GetRecentSensorEventsRequest(Guid DeviceId, int Limit = 50);

public sealed class GetRecentSensorEventsValidator : Validator<GetRecentSensorEventsRequest>
{
    public GetRecentSensorEventsValidator()
    {
        RuleFor(x => x.DeviceId).NotEmpty();
        RuleFor(x => x.Limit).InclusiveBetween(1, 100);
    }
}
