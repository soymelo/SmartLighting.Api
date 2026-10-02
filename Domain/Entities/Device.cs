 namespace SmartLighting.Api.Domain.Entities;

public sealed class Device
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public Guid ProjectId { get; set; }
    public DateTime? LastTelemetryAtUtc { get; set; }

    public Project Project { get; set; } = null!;
    public ICollection<Sensor> Sensors { get; set; } = new List<Sensor>();
    public ICollection<Luminaire> Luminaires { get; set; } = new List<Luminaire>();
    public ICollection<SensorEvent> SensorEvents { get; set; } = new List<SensorEvent>();
    public ICollection<LuminaireStateChange> LuminaireStateChanges { get; set; } = new List<LuminaireStateChange>();
}
