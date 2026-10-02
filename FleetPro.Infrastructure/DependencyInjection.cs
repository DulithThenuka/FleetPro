using FleetPro.Application.Interfaces;
using FleetPro.Infrastructure.Data;
using FleetPro.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FleetPro.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("FleetProDatabase");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "FleetPro database connection string was not found.");
        }

        services.AddDbContext<FleetProDbContext>(options =>
            options.UseSqlServer(connectionString));


        // Repository registrations
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<
    IVehicleRepository,
    VehicleRepository>();

    services.AddScoped<IDriverRepository, DriverRepository>();


        return services;
    }
}