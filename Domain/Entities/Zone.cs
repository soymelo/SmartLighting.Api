namespace SmartLighting.Api.Domain.Entities;

public sealed class Zone
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }

    public Project Project { get; set; } = null!;
    public ICollection<ZoneLuminaire> ZoneLuminaires { get; set; } = new List<ZoneLuminaire>();
    public ICollection<Sensor> Sensors { get; set; } = new List<Sensor>();
}
