namespace SmartLighting.Api.Domain.Entities;

public sealed class Sensor
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ZoneId { get; set; }
    public string Name { get; set; } = string.Empty;

    public Zone Zone { get; set; } = null!;
}
