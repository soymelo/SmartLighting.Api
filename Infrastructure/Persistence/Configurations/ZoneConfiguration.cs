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

        builder.Property(zone => zone.ZoneIdentifier)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(zone => new { zone.DeviceId, zone.ZoneIdentifier })
            .IsUnique();

        builder.HasIndex(zone => new { zone.DeviceId, zone.DisplayOrder })
            .IsUnique();

        builder.HasMany(zone => zone.Luminaires)
            .WithOne(luminaire => luminaire.Zone)
            .HasForeignKey(luminaire => luminaire.ZoneId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
