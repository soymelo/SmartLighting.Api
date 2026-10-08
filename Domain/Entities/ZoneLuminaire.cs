namespace SmartLighting.Api.Domain.Entities;

public sealed class ZoneLuminaire
{
    public Guid ZoneId { get; set; }
    public Guid LuminaireId { get; set; }

    public Zone Zone { get; set; } = null!;
    public Luminaire Luminaire { get; set; } = null!;
}
