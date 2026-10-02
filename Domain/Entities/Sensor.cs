namespace SmartLighting.Api.Domain.Entities;

public sealed class Sensor
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ZoneId { get; set; }
    public Guid DeviceId { get; set; }
    public string Name { get; set; } = string.Empty;

    public Zone Zone { get; set; } = null!;
    public Device Device { get; set; } = null!;
    public ICollection<SensorEvent> Events { get; set; } = new List<SensorEvent>();
}
