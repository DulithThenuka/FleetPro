using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FleetPro.Infrastructure.Data;

public class FleetProDbContextFactory
    : IDesignTimeDbContextFactory<FleetProDbContext>
{
    public FleetProDbContext CreateDbContext(string[] args)
    {
        const string connectionString =
            "Server=.\\TEW_SQLEXPRESS;Database=FleetProDb;Trusted_Connection=True;TrustServerCertificate=True;";

        var optionsBuilder = new DbContextOptionsBuilder<FleetProDbContext>();

        optionsBuilder.UseSqlServer(connectionString);

        return new FleetProDbContext(optionsBuilder.Options);
    }
}