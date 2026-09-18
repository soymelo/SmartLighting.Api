using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLighting.Api.Domain.Entities;

namespace SmartLighting.Api.Infrastructure.Persistence.Configurations;

public sealed class LuminaireStateConfiguration : IEntityTypeConfiguration<LuminaireState>
{
    public void Configure(EntityTypeBuilder<LuminaireState> builder)
    {
        builder.ToTable("LuminaireStates", table =>
            table.HasCheckConstraint(
                "CK_LuminaireStates_BrightnessPercentage",
                "[BrightnessPercentage] BETWEEN 0 AND 100"));

        builder.HasKey(state => state.LuminaireId);
    }
}
