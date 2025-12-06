using DustIntheWind.EfCoreMigrationsDemo.Business;
using DustIntheWind.EfCoreMigrationsDemo.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DustIntheWind.EfCoreMigrationsDemo;

internal static class Setup
{
    public static void ConfigureServices(ServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContext<DemoDbContext>(static options =>
        {
            // In a real application, make sure to retrieve the connection string from a secure configuration source.
            const string connectionString = "Server=localhost;Database=EfCoreMigrationsDemo;Trusted_Connection=true;TrustServerCertificate=True";

            options.UseSqlServer(connectionString);
        });

        serviceCollection.AddTransient<CreateOrderUseCase>();
        serviceCollection.AddTransient<GetOrdersUseCase>();
    }
}
