using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLighting.Api.Domain.Entities;

namespace SmartLighting.Api.Infrastructure.Persistence.Configurations;

public sealed class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.ToTable("Devices");

        builder.HasKey(device => device.Id);

        builder.Property(device => device.DeviceIdentifier)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(device => device.DeviceIdentifier)
            .IsUnique();

        builder.HasMany(device => device.Zones)
            .WithOne(zone => zone.Device)
            .HasForeignKey(zone => zone.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(device => device.Sensors)
            .WithOne(sensor => sensor.Device)
            .HasForeignKey(sensor => sensor.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
