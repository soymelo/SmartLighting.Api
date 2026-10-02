using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLighting.Api.Domain.Entities;

namespace SmartLighting.Api.Infrastructure.Persistence.Configurations;

public sealed class SensorEventConfiguration : IEntityTypeConfiguration<SensorEvent>
{
    public void Configure(EntityTypeBuilder<SensorEvent> builder)
    {
        builder.ToTable("SensorEvents");

        builder.HasKey(sensorEvent => sensorEvent.Id);

        builder.Property(sensorEvent => sensorEvent.DeviceId)
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);

        builder.HasIndex(sensorEvent => sensorEvent.EventIdentifier)
            .IsUnique();

        builder.HasIndex(sensorEvent => new { sensorEvent.SensorId, sensorEvent.OccurredAtUtc });

        builder.HasOne(sensorEvent => sensorEvent.Device)
            .WithMany(device => device.SensorEvents)
            .HasForeignKey(sensorEvent => sensorEvent.DeviceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
