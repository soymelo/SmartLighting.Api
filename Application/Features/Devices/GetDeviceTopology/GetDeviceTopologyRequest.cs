using FastEndpoints;
using FluentValidation;

namespace SmartLighting.Api.Application.Features.Devices.GetDeviceTopology;

public sealed class GetDeviceTopologyRequest
{
    public string DeviceIdentifier { get; init; } = string.Empty;
}

public sealed class GetDeviceTopologyValidator : Validator<GetDeviceTopologyRequest>
{
    public GetDeviceTopologyValidator()
    {
        RuleFor(request => request.DeviceIdentifier)
            .NotEmpty()
            .MaximumLength(50);
    }
}
