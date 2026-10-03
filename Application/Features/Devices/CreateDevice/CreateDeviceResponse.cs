namespace SmartLighting.Api.Application.Features.Devices.CreateDevice;

public sealed record CreateDeviceResponse(
    Guid DeviceId,
    string Name,
    int SensorsCreated,
    int LuminairesCreated);
