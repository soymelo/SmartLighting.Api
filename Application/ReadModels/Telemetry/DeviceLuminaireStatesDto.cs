namespace SmartLighting.Api.Application.ReadModels.Telemetry;

public sealed record DeviceLuminaireStatesDto(
    Guid DeviceId,
    IReadOnlyCollection<DeviceLuminaireStateDto> Luminaires);

public sealed record DeviceLuminaireStateDto(
    Guid LuminaireId,
    int? BrightnessPercentage,
    DateTime? UpdatedAtUtc);
