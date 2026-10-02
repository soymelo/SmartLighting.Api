namespace SmartLighting.Api.Domain.Entities;

public sealed class Project
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;

    public ICollection<Device> Devices { get; set; } = new List<Device>();
    public ICollection<Zone> Zones { get; set; } = new List<Zone>();
}
