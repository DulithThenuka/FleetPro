using FleetPro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetPro.Infrastructure.Data.Configurations;

public class VehicleConfiguration
    : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(
        EntityTypeBuilder<Vehicle> builder)
    {
        builder.HasKey(x => x.VehicleId);

        builder.Property(x => x.RegistrationNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(x => x.RegistrationNumber)
            .IsUnique();

        builder.Property(x => x.VIN)
            .HasMaxLength(50);

        builder.HasIndex(x => x.VIN)
            .IsUnique()
            .HasFilter("[VIN] IS NOT NULL");

        builder.Property(x => x.EngineNumber)
            .HasMaxLength(50);

        builder.Property(x => x.Brand)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Model)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Color)
            .HasMaxLength(50);

        builder.Property(x => x.PurchasePrice)
            .HasPrecision(18, 2);

        builder.Property(x => x.CurrentMileage)
            .HasPrecision(12, 1);

        builder.Property(x => x.FuelType)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(x => x.Transmission)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.HasOne(x => x.Branch)
            .WithMany(x => x.Vehicles)
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.VehicleType)
            .WithMany(x => x.Vehicles)
            .HasForeignKey(x => x.VehicleTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}