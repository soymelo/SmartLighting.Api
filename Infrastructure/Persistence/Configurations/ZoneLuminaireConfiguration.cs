using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLighting.Api.Domain.Entities;

namespace SmartLighting.Api.Infrastructure.Persistence.Configurations;

public sealed class ZoneLuminaireConfiguration : IEntityTypeConfiguration<ZoneLuminaire>
{
    public void Configure(EntityTypeBuilder<ZoneLuminaire> builder)
    {
        builder.ToTable("ZoneLuminaires");

        builder.HasKey(zoneLuminaire => new { zoneLuminaire.ZoneId, zoneLuminaire.LuminaireId });

        builder.HasOne(zoneLuminaire => zoneLuminaire.Zone)
            .WithMany(zone => zone.ZoneLuminaires)
            .HasForeignKey(zoneLuminaire => zoneLuminaire.ZoneId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(zoneLuminaire => zoneLuminaire.Luminaire)
            .WithMany(luminaire => luminaire.ZoneLuminaires)
            .HasForeignKey(zoneLuminaire => zoneLuminaire.LuminaireId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
