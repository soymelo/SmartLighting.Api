using Microsoft.EntityFrameworkCore;
using SmartLighting.Api.Domain.Entities;

namespace SmartLighting.Api.Infrastructure.Persistence;

public sealed class SmartLightingDbContext(DbContextOptions<SmartLightingDbContext> options) : DbContext(options)
{
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<Zone> Zones => Set<Zone>();
    public DbSet<ZoneLuminaire> ZoneLuminaires => Set<ZoneLuminaire>();
    public DbSet<Luminaire> Luminaires => Set<Luminaire>();
    public DbSet<Sensor> Sensors => Set<Sensor>();
    public DbSet<SensorEvent> SensorEvents => Set<SensorEvent>();
    public DbSet<LuminaireStateChange> LuminaireStateChanges => Set<LuminaireStateChange>();
    public DbSet<LuminaireState> LuminaireStates => Set<LuminaireState>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SmartLightingDbContext).Assembly);
    }
}
