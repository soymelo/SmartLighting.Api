namespace SmartLighting.Api.Application.ReadModels.Telemetry;

public sealed record DeviceTelemetryStatusDto(Guid DeviceId, DateTime? LastTelemetryAtUtc);
