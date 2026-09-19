namespace SmartLighting.Api.Application.Features.Devices.GetDevices;

public sealed record GetDevicesResponse(IReadOnlyCollection<GetDevicesResponse.DeviceResponse> Devices)
{
    public sealed record DeviceResponse(
        Guid DeviceId,
        string Name,
        DateTime? LastTelemetryAtUtc);
}
