namespace SmartLighting.Api.Application.Features.Telemetry.GetLuminaireStates;

public sealed record GetLuminaireStatesResponse(
    Guid DeviceId,
    IReadOnlyCollection<GetLuminaireStatesResponse.LuminaireResponse> Luminaires)
{
    public sealed record LuminaireResponse(
        Guid LuminaireId,
        int? BrightnessPercentage,
        DateTime? UpdatedAtUtc);
}
