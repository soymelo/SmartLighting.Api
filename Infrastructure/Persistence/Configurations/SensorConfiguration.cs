using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLighting.Api.Domain.Entities;

namespace SmartLighting.Api.Infrastructure.Persistence.Configurations;

public sealed class SensorConfiguration : IEntityTypeConfiguration<Sensor>
{
    public void Configure(EntityTypeBuilder<Sensor> builder)
    {
        builder.ToTable("Sensors");

        builder.HasKey(sensor => sensor.Id);

        builder.Property(sensor => sensor.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasMany(sensor => sensor.Events)
            .WithOne(sensorEvent => sensorEvent.Sensor)
            .HasForeignKey(sensorEvent => sensorEvent.SensorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
