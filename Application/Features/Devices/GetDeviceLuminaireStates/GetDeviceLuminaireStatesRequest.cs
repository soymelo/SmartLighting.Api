using FastEndpoints;
using FluentValidation;

namespace SmartLighting.Api.Application.Features.Devices.GetDeviceLuminaireStates;

public sealed record GetDeviceLuminaireStatesRequest(Guid DeviceId);

public sealed class GetDeviceLuminaireStatesValidator : Validator<GetDeviceLuminaireStatesRequest>
{
    public GetDeviceLuminaireStatesValidator()
    {
        RuleFor(x => x.DeviceId).NotEmpty();
    }
}
