namespace SmartLighting.Api.Application.Features.Telemetry.GetTelemetryStatus;

public sealed record GetTelemetryStatusResponse(Guid DeviceId, DateTime? LastTelemetryAtUtc);
