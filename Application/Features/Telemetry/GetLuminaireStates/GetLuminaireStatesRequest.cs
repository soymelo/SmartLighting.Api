using FastEndpoints;
using FluentValidation;

namespace SmartLighting.Api.Application.Features.Telemetry.GetLuminaireStates;

public sealed record GetLuminaireStatesRequest(Guid DeviceId);

public sealed class GetLuminaireStatesValidator : Validator<GetLuminaireStatesRequest>
{
    public GetLuminaireStatesValidator()
    {
        RuleFor(x => x.DeviceId).NotEmpty();
    }
}
