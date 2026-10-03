using FastEndpoints;
using FluentValidation;

namespace SmartLighting.Api.Application.Features.Devices.GetDeviceById;

public sealed record GetDeviceByIdRequest(Guid DeviceId);

public sealed class GetDeviceByIdValidator : Validator<GetDeviceByIdRequest>
{
    public GetDeviceByIdValidator()
    {
        RuleFor(x => x.DeviceId).NotEmpty();
    }
}
