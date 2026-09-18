 namespace SmartLighting.Api.Domain.Entities;

public sealed class Device
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;

    public ICollection<Zone> Zones { get; set; } = new List<Zone>();
}
