using DustIntheWind.EfCoreMigrationsDemo.Business;
using DustIntheWind.EfCoreMigrationsDemo.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DustIntheWind.EfCoreMigrationsDemo;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        ServiceCollection serviceCollection = new();
        Setup.ConfigureServices(serviceCollection);
        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        // Create one order

        using (IServiceScope serviceScope = serviceProvider.CreateScope())
            await CreateOrder(serviceScope.ServiceProvider);

        // Display all orders

        using (IServiceScope serviceScope = serviceProvider.CreateScope())
            await DisplayAllOrders(serviceScope.ServiceProvider);
    }

    private static async Task CreateOrder(IServiceProvider serviceProvider)
    {
        CreateOrderUseCase useCase = serviceProvider.GetRequiredService<CreateOrderUseCase>();
        await useCase.ExecuteAsync();
    }

    private static async Task DisplayAllOrders(IServiceProvider serviceProvider)
    {
        GetOrdersUseCase useCase = serviceProvider.GetRequiredService<GetOrdersUseCase>();
        List<Order> orders = await useCase.ExecuteAsync();

        DisplayOrders(orders);
    }

    private static void DisplayOrders(List<Order> orders)
    {
        foreach (Order order in orders)
        {
            Console.WriteLine($"Order:");
            Console.WriteLine($"  - Id: {order.Id}");
            Console.WriteLine($"  - Date: {order.Date}");
            Console.WriteLine($"  - Customer Id: {order.CustomerId}");

            if (order.Customer is not null)
            {
                Console.WriteLine($"  - Customer:");
                Console.WriteLine($"    - Id: {order.Customer.Id}");
                Console.WriteLine($"    - Name: {order.Customer.Name}");
            }
            else
            {
                Console.WriteLine("  - Customer: <null>");
            }

            Console.WriteLine();
        }
    }
}