namespace SmartLighting.Api.Domain.Entities;

public sealed class SensorEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EventIdentifier { get; set; }
    public Guid SensorId { get; set; }
    public DateTime OccurredAtUtc { get; set; }

    public Sensor Sensor { get; set; } = null!;
}
