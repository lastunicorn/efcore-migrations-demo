using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DustIntheWind.EfCoreMigrationsDemo.DataAccess;

internal class DemoDbContextFactory : IDesignTimeDbContextFactory<DemoDbContext>
{
    public DemoDbContext CreateDbContext(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        string connectionString = configuration.GetConnectionString("DefaultConnection");

        DbContextOptionsBuilder<DemoDbContext> optionsBuilder = new();
        optionsBuilder.UseSqlServer(connectionString);

        return new DemoDbContext(optionsBuilder.Options);
    }
}