using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Gym.Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<GymDbContext>
{
    public GymDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<GymDbContext>();
        optionsBuilder.UseSqlServer("Server=localhost;Database=GymManagementDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");

        return new GymDbContext(optionsBuilder.Options);
    }
}
