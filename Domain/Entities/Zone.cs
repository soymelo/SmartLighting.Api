namespace SmartLighting.Api.Domain.Entities;

public sealed class Zone
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid DeviceId { get; set; }
    public string ZoneIdentifier { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }

    public Device Device { get; set; } = null!;
    public ICollection<Luminaire> Luminaires { get; set; } = new List<Luminaire>();
}
