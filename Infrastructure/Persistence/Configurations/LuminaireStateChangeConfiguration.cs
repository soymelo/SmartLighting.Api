using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLighting.Api.Domain.Entities;

namespace SmartLighting.Api.Infrastructure.Persistence.Configurations;

public sealed class LuminaireStateChangeConfiguration : IEntityTypeConfiguration<LuminaireStateChange>
{
    public void Configure(EntityTypeBuilder<LuminaireStateChange> builder)
    {
        builder.ToTable("LuminaireStateChanges", table =>
            table.HasCheckConstraint(
                "CK_LuminaireStateChanges_BrightnessPercentage",
                "[BrightnessPercentage] BETWEEN 0 AND 100"));

        builder.HasKey(stateChange => stateChange.Id);

        builder.Property(stateChange => stateChange.DeviceId)
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);

        builder.HasIndex(stateChange => stateChange.EventIdentifier)
            .IsUnique();

        builder.HasIndex(stateChange => new { stateChange.LuminaireId, stateChange.ChangedAtUtc });

        builder.HasOne(stateChange => stateChange.Device)
            .WithMany(device => device.LuminaireStateChanges)
            .HasForeignKey(stateChange => stateChange.DeviceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
