namespace SmartLighting.Api.Domain.Entities;

public sealed class LuminaireState
{
    public Guid LuminaireId { get; set; }
    public int BrightnessPercentage { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    public Luminaire Luminaire { get; set; } = null!;
}
