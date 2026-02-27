using DustIntheWind.EfCoreMigrationsDemo.Business;
using DustIntheWind.EfCoreMigrationsDemo.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DustIntheWind.EfCoreMigrationsDemo;

/// <summary>
/// 00 - Initial project setup (with Entity Framework)
/// ============================================
/// 
/// At this stage, we have a simple application with two use cases:
/// - CreateOrderUseCase: creates a new order with a customer.
/// - GetOrdersUseCase: retrieves and displays all orders with their associated customers.
/// 
/// The application uses Entity Framework Core for data access.
/// 
/// There is no logic in place to update database schema yet.
/// The next steps will involve creating and applying migrations to set up the database schema.
/// </summary>
internal static class Program
{
    private static async Task Main(string[] args)
    {
        ServiceCollection serviceCollection = new();
        Setup.ConfigureServices(serviceCollection);
        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        switch(args.FirstOrDefault())
        {
            case "create":
                await CreateOrder(serviceProvider);
                break;

            case "display":
                await DisplayAllOrders(serviceProvider);
                break;

            default:
                Console.WriteLine("Usage: dotnet run [create|display]");
                break;
        }
    }

    private static async Task CreateOrder(IServiceProvider serviceProvider)
    {
        using IServiceScope serviceScope = serviceProvider.CreateScope();

        CreateOrderUseCase useCase = serviceScope.ServiceProvider.GetRequiredService<CreateOrderUseCase>();
        await useCase.ExecuteAsync();
    }

    private static async Task DisplayAllOrders(IServiceProvider serviceProvider)
    {
        using IServiceScope serviceScope = serviceProvider.CreateScope();

        GetOrdersUseCase useCase = serviceScope.ServiceProvider.GetRequiredService<GetOrdersUseCase>();
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