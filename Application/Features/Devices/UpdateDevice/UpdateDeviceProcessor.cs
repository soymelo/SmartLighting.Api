using FastEndpoints;
using SmartLighting.Api.Application.Commands.Devices;

namespace SmartLighting.Api.Application.Features.Devices.UpdateDevice;

public sealed class UpdateDeviceProcessor
{
    public Task<UpdateDeviceResponse> HandleAsync(UpdateDeviceRequest request, CancellationToken ct)
    {
        var command = new UpdateDeviceCommand(
            request.DeviceId,
            request.Name,
            request.Sensors,
            request.Luminaires);

        return command.ExecuteAsync(ct);
    }
}
