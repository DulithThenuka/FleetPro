using FleetPro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FleetPro.Infrastructure.Data.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(
        IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var context =
            scope.ServiceProvider
            .GetRequiredService<FleetProDbContext>();

        await context.Database.MigrateAsync();


        // Seed Roles
        if (!await context.Roles.AnyAsync())
        {
            var roles = new List<Role>
            {
                new()
                {
                    RoleName = "Administrator",
                    Description = "Full system access"
                },

                new()
                {
                    RoleName = "Fleet Manager",
                    Description = "Fleet operation management"
                },

                new()
                {
                    RoleName = "Mechanic",
                    Description = "Vehicle maintenance access"
                },

                new()
                {
                    RoleName = "Driver",
                    Description = "Driver access"
                }
            };


            await context.Roles.AddRangeAsync(roles);

            await context.SaveChangesAsync();
        }

        if (!await context.Branches.AnyAsync())
{
    var branch = new Branch
    {
        BranchCode = "HQ",
        BranchName = "Head Office",
        Address = "Main Office",
        IsActive = true
    };

    await context.Branches.AddAsync(branch);

    await context.SaveChangesAsync();
}

if (!await context.VehicleTypes.AnyAsync())
{
    var vehicleTypes = new List<VehicleType>
    {
        new()
        {
            Name = "Car",
            Description = "Passenger car"
        },
        new()
        {
            Name = "Van",
            Description = "Passenger or cargo van"
        },
        new()
        {
            Name = "Truck",
            Description = "Commercial truck"
        },
        new()
        {
            Name = "Bus",
            Description = "Passenger bus"
        },
        new()
        {
            Name = "Motorcycle",
            Description = "Motorcycle"
        }
    };

    await context.VehicleTypes.AddRangeAsync(vehicleTypes);

    await context.SaveChangesAsync();
}



        // Seed Admin User
        if (!await context.Users.AnyAsync())
        {
            var adminRole =
                await context.Roles
                .FirstAsync(x =>
                    x.RoleName == "Administrator");


            var adminUser = new User
            {
                Username = "admin",

                PasswordHash =
                    BCrypt.Net.BCrypt.HashPassword(
                        "Admin@123"),

                FullName = "System Administrator",

                Email = "admin@fleetpro.com",

                IsActive = true
            };


            adminUser.UserRoles.Add(
                new UserRole
                {
                    Role = adminRole
                });


            await context.Users.AddAsync(adminUser);

            await context.SaveChangesAsync();
        }
    }
}