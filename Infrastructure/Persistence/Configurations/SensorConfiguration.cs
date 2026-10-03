using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLighting.Api.Domain.Entities;

namespace SmartLighting.Api.Infrastructure.Persistence.Configurations;

public sealed class SensorConfiguration : IEntityTypeConfiguration<Sensor>
{
    public void Configure(EntityTypeBuilder<Sensor> builder)
    {
        builder.ToTable("Sensors", table =>
            table.HasCheckConstraint("CK_Sensors_Slot", "[Slot] IN (1, 2, 3)"));

        builder.HasKey(sensor => sensor.Id);

        builder.Property(sensor => sensor.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(sensor => sensor.Slot)
            .HasConversion<int>()
            .IsRequired();

        builder.HasIndex(sensor => new { sensor.DeviceId, sensor.Slot })
            .IsUnique();

        builder.HasOne(sensor => sensor.Device)
            .WithMany(device => device.Sensors)
            .HasForeignKey(sensor => sensor.DeviceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(sensor => sensor.Events)
            .WithOne(sensorEvent => sensorEvent.Sensor)
            .HasForeignKey(sensorEvent => sensorEvent.SensorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
