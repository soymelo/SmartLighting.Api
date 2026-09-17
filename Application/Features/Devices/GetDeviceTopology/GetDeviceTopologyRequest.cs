using FastEndpoints;
using FluentValidation;

namespace SmartLighting.Api.Application.Features.Devices.GetDeviceTopology;

public sealed record GetDeviceTopologyRequest(string DeviceIdentifier);

public sealed class GetDeviceTopologyValidator : Validator<GetDeviceTopologyRequest>
{
    public GetDeviceTopologyValidator()
    {
        RuleFor(x => x.DeviceIdentifier)
            .NotEmpty()
            .MaximumLength(50);
    }
}
