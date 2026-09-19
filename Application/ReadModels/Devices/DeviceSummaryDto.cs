namespace SmartLighting.Api.Application.ReadModels.Devices;

public sealed record DeviceSummaryDto(
    Guid DeviceId,
    string Name,
    DateTime? LastTelemetryAtUtc);
