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
        Setup.ConfigureServices(serviceCollection);
        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();


    }

    {



    }
}