namespace SmartLighting.Api.Domain.Entities;

public sealed class LuminaireStateChange
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EventIdentifier { get; set; }
    public Guid LuminaireId { get; set; }
    public int BrightnessPercentage { get; set; }
    public DateTime ChangedAtUtc { get; set; }

    public Luminaire Luminaire { get; set; } = null!;
}
