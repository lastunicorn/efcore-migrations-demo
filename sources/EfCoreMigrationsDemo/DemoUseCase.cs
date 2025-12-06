using DustIntheWind.EfCoreMigrationsDemo.DataAccess;
using DustIntheWind.EfCoreMigrationsDemo.Domain;

namespace DustIntheWind.EfCoreMigrationsDemo;

internal class DemoUseCase
{
    private readonly DemoDbContext demoDbContext;

    public DemoUseCase(DemoDbContext demoDbContext)
    {
        this.demoDbContext = demoDbContext ?? throw new ArgumentNullException(nameof(demoDbContext));
    }

    public Task Execute()
    {
        Customer customer = demoDbContext.Customers
            .FirstOrDefault();

        Order order = new()
        {
            Customer = customer,
            ProductName = "Sample Product"
        };

        demoDbContext.Orders.Add(order);

        return demoDbContext.SaveChangesAsync();
    }
}
