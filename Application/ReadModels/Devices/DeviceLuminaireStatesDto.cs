namespace SmartLighting.Api.Application.ReadModels.Devices;

public sealed record DeviceLuminaireStatesDto(
    Guid DeviceId,
    IReadOnlyCollection<DeviceLuminaireStateDto> Luminaires);

public sealed record DeviceLuminaireStateDto(
    Guid LuminaireId,
    int? BrightnessPercentage,
    DateTime? UpdatedAtUtc);
