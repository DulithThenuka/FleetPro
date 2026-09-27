using FleetPro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetPro.Infrastructure.Data.Configurations;

public class VehicleTypeConfiguration
    : IEntityTypeConfiguration<VehicleType>
{
    public void Configure(
        EntityTypeBuilder<VehicleType> builder)
    {
        builder.HasKey(x => x.VehicleTypeId);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Description)
            .HasMaxLength(255);

        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}