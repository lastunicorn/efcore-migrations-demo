using DustIntheWind.EfCoreMigrationsDemo.DataAccess;
using DustIntheWind.EfCoreMigrationsDemo.Domain;

namespace DustIntheWind.EfCoreMigrationsDemo.Business;

internal class CreateOrderUseCase
{
    private static readonly string[] firstNames = ["John", "Jane", "Michael", "Emily", "David", "Sarah"];
    private static readonly string[] lastNames = ["Doe", "Smith", "Johnson", "Brown", "Davis", "Miller"];

    private readonly DemoDbContext demoDbContext;

    public CreateOrderUseCase(DemoDbContext demoDbContext)
    {
        this.demoDbContext = demoDbContext ?? throw new ArgumentNullException(nameof(demoDbContext));
    }

    public Task ExecuteAsync()
    {
        Order order = new()
        {
            Date = GenerateRandomDate(),
            Customer = CreateRandomCustomer()
        };

        _ = demoDbContext.Orders.Add(order);

        return demoDbContext.SaveChangesAsync();
    }

    private static DateTime GenerateRandomDate()
    {
        int orderDaysOld = Random.Shared.Next(100);

        return DateTime.UtcNow.AddDays(-orderDaysOld);
    }

    private static Customer CreateRandomCustomer()
    {
        int firstnameIndex = Random.Shared.Next(firstNames.Length);
        int lastnameIndex = Random.Shared.Next(lastNames.Length);

        return new Customer()
        {
            Name = firstNames[firstnameIndex] + " " + lastNames[lastnameIndex]
        };
    }
}
