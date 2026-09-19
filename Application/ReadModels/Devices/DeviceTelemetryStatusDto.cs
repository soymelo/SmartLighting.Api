namespace SmartLighting.Api.Application.ReadModels.Devices;

public sealed record DeviceTelemetryStatusDto(Guid DeviceId, DateTime? LastTelemetryAtUtc);
