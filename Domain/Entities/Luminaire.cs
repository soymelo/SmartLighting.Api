using SmartLighting.Api.Domain.Hardware;

namespace SmartLighting.Api.Domain.Entities;

public sealed class Luminaire
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid DeviceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public LuminaireSlot Slot { get; set; }
    public decimal NominalPowerWatts { get; set; }

    public ICollection<ZoneLuminaire> ZoneLuminaires { get; set; } = new List<ZoneLuminaire>();
    public Device Device { get; set; } = null!;
    public LuminaireState? State { get; set; }
    public ICollection<LuminaireStateChange> StateChanges { get; set; } = new List<LuminaireStateChange>();
}
