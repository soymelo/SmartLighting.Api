using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLighting.Api.Domain.Entities;

namespace SmartLighting.Api.Infrastructure.Persistence.Configurations;

public sealed class LuminaireConfiguration : IEntityTypeConfiguration<Luminaire>
{
    public void Configure(EntityTypeBuilder<Luminaire> builder)
    {
        builder.ToTable("Luminaires", table =>
            table.HasCheckConstraint("CK_Luminaires_NominalPowerWatts", "[NominalPowerWatts] > 0"));

        builder.HasKey(luminaire => luminaire.Id);

        builder.Property(luminaire => luminaire.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(luminaire => luminaire.NominalPowerWatts)
            .HasPrecision(10, 2);

    }
}
