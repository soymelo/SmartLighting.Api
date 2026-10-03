namespace SmartLighting.Api.Application.ReadModels.Devices;

public sealed record DeviceSummaryDto(
    Guid DeviceId,
    string Name,
    Guid ProjectId,
    string ProjectName,
    DateTime? LastTelemetryAtUtc);
