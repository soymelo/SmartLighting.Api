using FastEndpoints;
using SmartLighting.Api.Application.Abstractions.Queries.Devices;
using SmartLighting.Api.Application.Features.Devices.GetDevices;

namespace SmartLighting.Api.Application.Queries.Devices;

public sealed class GetDevicesHandler(IDeviceQueryRepository deviceQueryRepository)
    : ICommandHandler<GetDevicesQuery, GetDevicesResponse>
{
    public async Task<GetDevicesResponse> ExecuteAsync(GetDevicesQuery query, CancellationToken ct)
    {
        var devices = await deviceQueryRepository.GetAllAsync(ct);
        var responses = devices
            .Select(device => new GetDevicesResponse.DeviceResponse(
                device.DeviceId,
                device.Name,
                device.LastTelemetryAtUtc)).ToList();

        return new GetDevicesResponse(responses);
    }
}
