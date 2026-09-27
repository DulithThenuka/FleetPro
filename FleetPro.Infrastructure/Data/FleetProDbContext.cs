using FleetPro.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FleetPro.Infrastructure.Data;

public class FleetProDbContext : DbContext
{
    public FleetProDbContext(DbContextOptions<FleetProDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<Permission> Permissions => Set<Permission>();

    public DbSet<UserRole> UserRoles => Set<UserRole>();

    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

    public DbSet<Branch> Branches => Set<Branch>();

    public DbSet<VehicleType> VehicleTypes => Set<VehicleType>();

    public DbSet<Vehicle> Vehicles => Set<Vehicle>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserRole>()
            .HasKey(x => new { x.UserId, x.RoleId });

        modelBuilder.Entity<RolePermission>()
            .HasKey(x => new { x.RoleId, x.PermissionId });

        modelBuilder.Entity<User>()
            .HasIndex(x => x.Username)
            .IsUnique();

        modelBuilder.Entity<Role>()
            .HasIndex(x => x.RoleName)
            .IsUnique();

        modelBuilder.Entity<Permission>()
            .HasIndex(x => x.PermissionName)
            .IsUnique();

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(FleetProDbContext).Assembly);
    }
}