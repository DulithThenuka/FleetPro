using FleetPro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetPro.Infrastructure.Data.Configurations;

public class DriverConfiguration
    : IEntityTypeConfiguration<Driver>
{
    public void Configure(
        EntityTypeBuilder<Driver> builder)
    {
        builder.HasKey(x => x.DriverId);

        builder.Property(x => x.EmployeeNumber)
            .IsRequired()
            .HasMaxLength(30);

        builder.HasIndex(x => x.EmployeeNumber)
            .IsUnique();

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.NIC)
            .HasMaxLength(20);

        builder.Property(x => x.Phone)
            .HasMaxLength(20);

        builder.Property(x => x.Address)
            .HasMaxLength(255);

        builder.Property(x => x.LicenseNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.LicenseNumber)
            .IsUnique();

        builder.Property(x => x.LicenseCategory)
            .HasMaxLength(50);

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.HasOne(x => x.Branch)
            .WithMany(x => x.Drivers)
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}