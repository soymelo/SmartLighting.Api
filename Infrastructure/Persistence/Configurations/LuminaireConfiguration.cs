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

        builder.HasOne(luminaire => luminaire.Device)
            .WithMany(device => device.Luminaires)
            .HasForeignKey(luminaire => luminaire.DeviceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(luminaire => luminaire.StateChanges)
            .WithOne(stateChange => stateChange.Luminaire)
            .HasForeignKey(stateChange => stateChange.LuminaireId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(luminaire => luminaire.State)
            .WithOne(state => state.Luminaire)
            .HasForeignKey<LuminaireState>(state => state.LuminaireId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
