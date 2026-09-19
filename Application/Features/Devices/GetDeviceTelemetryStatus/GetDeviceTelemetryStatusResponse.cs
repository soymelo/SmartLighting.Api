namespace SmartLighting.Api.Application.Features.Devices.GetDeviceTelemetryStatus;

public sealed record GetDeviceTelemetryStatusResponse(Guid DeviceId, DateTime? LastTelemetryAtUtc);
