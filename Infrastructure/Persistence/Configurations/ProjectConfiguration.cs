using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLighting.Api.Domain.Entities;

namespace SmartLighting.Api.Infrastructure.Persistence.Configurations;

public sealed class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");

        builder.HasKey(project => project.Id);

        builder.Property(project => project.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasMany(project => project.Devices)
            .WithOne(device => device.Project)
            .HasForeignKey(device => device.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(project => project.Zones)
            .WithOne(zone => zone.Project)
            .HasForeignKey(zone => zone.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
