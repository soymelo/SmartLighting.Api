 namespace SmartLighting.Api.Domain.Entities;

public sealed class Device
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public DateTime? LastTelemetryAtUtc { get; set; }

    public ICollection<Zone> Zones { get; set; } = new List<Zone>();
}
