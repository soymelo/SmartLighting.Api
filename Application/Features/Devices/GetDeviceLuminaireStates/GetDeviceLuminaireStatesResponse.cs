namespace SmartLighting.Api.Application.Features.Devices.GetDeviceLuminaireStates;

public sealed record GetDeviceLuminaireStatesResponse(
    Guid DeviceId,
    IReadOnlyCollection<GetDeviceLuminaireStatesResponse.LuminaireResponse> Luminaires)
{
    public sealed record LuminaireResponse(
        Guid LuminaireId,
        int? BrightnessPercentage,
        DateTime? UpdatedAtUtc);
}
