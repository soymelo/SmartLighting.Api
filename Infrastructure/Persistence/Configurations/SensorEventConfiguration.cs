using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLighting.Api.Domain.Entities;

namespace SmartLighting.Api.Infrastructure.Persistence.Configurations;

public sealed class SensorEventConfiguration : IEntityTypeConfiguration<SensorEvent>
{
    public void Configure(EntityTypeBuilder<SensorEvent> builder)
    {
        builder.ToTable("SensorEvents");

        builder.HasKey(sensorEvent => sensorEvent.Id);

        builder.HasIndex(sensorEvent => sensorEvent.EventIdentifier)
            .IsUnique();

        builder.HasIndex(sensorEvent => new { sensorEvent.SensorId, sensorEvent.OccurredAtUtc });
    }
}
