using DustIntheWind.EfCoreMigrationsDemo.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DustIntheWind.EfCoreMigrationsDemo;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        ServiceCollection serviceCollection = new();
        ConfigureServices(serviceCollection);
        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        using DemoDbContext dbContext = serviceProvider.GetRequiredService<DemoDbContext>();
        dbContext.Database.EnsureCreated();

        DemoUseCase useCase = serviceProvider.GetRequiredService<DemoUseCase>();
        await useCase.Execute();
    }

    private static void ConfigureServices(ServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContext<DemoDbContext>(optionsBuilder =>
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);
        });

        serviceCollection.AddTransient<DemoUseCase>();
    }
}