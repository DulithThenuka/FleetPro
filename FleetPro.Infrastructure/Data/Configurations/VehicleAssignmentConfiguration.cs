using FleetPro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetPro.Infrastructure.Data.Configurations;

public class VehicleAssignmentConfiguration
    : IEntityTypeConfiguration<VehicleAssignment>
{
    public void Configure(
        EntityTypeBuilder<VehicleAssignment> builder)
    {
        builder.HasKey(x => x.AssignmentId);

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(x => x.StartMileage)
            .HasPrecision(12, 1);

        builder.Property(x => x.EndMileage)
            .HasPrecision(12, 1);

        builder.Property(x => x.Notes)
            .HasMaxLength(500);

        builder.HasOne(x => x.Vehicle)
            .WithMany(x => x.VehicleAssignments)
            .HasForeignKey(x => x.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Driver)
            .WithMany(x => x.VehicleAssignments)
            .HasForeignKey(x => x.DriverId)
            .OnDelete(DeleteBehavior.Restrict);

        // Only one active assignment per vehicle.
        builder.HasIndex(x => new
            {
                x.VehicleId,
                x.Status
            })
            .HasFilter("[Status] = 'Active'")
            .IsUnique();

        // Only one active vehicle assignment per driver.
        builder.HasIndex(x => new
            {
                x.DriverId,
                x.Status
            })
            .HasFilter("[Status] = 'Active'")
            .IsUnique();
    }
}