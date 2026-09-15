namespace SmartLighting.Api.Domain.Entities;

public sealed class Sensor
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid DeviceId { get; set; }
    public Guid ZoneId { get; set; }
    public string SensorIdentifier { get; set; } = string.Empty;

    public Device Device { get; set; } = null!;
    public Zone Zone { get; set; } = null!;
}
