namespace SmartLighting.Api.Domain.Entities;

public sealed class LuminaireState
{
    public Guid LuminaireId { get; set; }
    public int BrightnessPercentage { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    public Luminaire Luminaire { get; set; } = null!;

    public bool TryUpdate(int brightnessPercentage, DateTime updatedAtUtc)
    {
        if (updatedAtUtc <= UpdatedAtUtc)
        {
            return false;
        }

        BrightnessPercentage = brightnessPercentage;
        UpdatedAtUtc = updatedAtUtc;

        return true;
    }
}
