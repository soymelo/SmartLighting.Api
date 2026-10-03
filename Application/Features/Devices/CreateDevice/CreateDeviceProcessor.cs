using FastEndpoints;
using SmartLighting.Api.Application.Commands.Devices;

namespace SmartLighting.Api.Application.Features.Devices.CreateDevice;

public sealed class CreateDeviceProcessor
{
    public Task<CreateDeviceCommandResult> HandleAsync(
        CreateDeviceRequest request,
        CancellationToken ct)
    {
        var command = new CreateDeviceCommand(
            request.ProjectId,
            request.Name,
            request.Sensors,
            request.Luminaires);

        return command.ExecuteAsync(ct);
    }
}
