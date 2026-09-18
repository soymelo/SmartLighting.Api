using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLighting.Api.Domain.Entities;

namespace SmartLighting.Api.Infrastructure.Persistence.Configurations;

public sealed class ZoneConfiguration : IEntityTypeConfiguration<Zone>
{
    public void Configure(EntityTypeBuilder<Zone> builder)
    {
        builder.ToTable("Zones");

        builder.HasKey(zone => zone.Id);

        builder.Property(zone => zone.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(zone => new { zone.DeviceId, zone.DisplayOrder })
            .IsUnique();

        builder.HasMany(zone => zone.Luminaires)
            .WithOne(luminaire => luminaire.Zone)
            .HasForeignKey(luminaire => luminaire.ZoneId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(zone => zone.Sensors)
            .WithOne(sensor => sensor.Zone)
            .HasForeignKey(sensor => sensor.ZoneId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
