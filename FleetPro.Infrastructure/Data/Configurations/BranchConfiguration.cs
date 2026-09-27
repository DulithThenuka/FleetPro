using FleetPro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetPro.Infrastructure.Data.Configurations;

public class BranchConfiguration
    : IEntityTypeConfiguration<Branch>
{
    public void Configure(
        EntityTypeBuilder<Branch> builder)
    {
        builder.HasKey(x => x.BranchId);

        builder.Property(x => x.BranchCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.BranchName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Address)
            .HasMaxLength(255);

        builder.Property(x => x.Phone)
            .HasMaxLength(20);

        builder.Property(x => x.Email)
            .HasMaxLength(150);

        builder.HasIndex(x => x.BranchCode)
            .IsUnique();
    }
}