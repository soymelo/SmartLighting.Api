namespace SmartLighting.Api.Domain.Entities;

public sealed class Luminaire
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ZoneId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal NominalPowerWatts { get; set; }

    public Zone Zone { get; set; } = null!;
}
