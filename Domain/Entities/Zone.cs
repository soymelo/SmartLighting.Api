namespace SmartLighting.Api.Domain.Entities;

public sealed class Zone
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }

    public Project Project { get; set; } = null!;
    public ICollection<Luminaire> Luminaires { get; set; } = new List<Luminaire>();
    public ICollection<Sensor> Sensors { get; set; } = new List<Sensor>();
}
