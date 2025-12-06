using DustIntheWind.EfCoreMigrationsDemo.DataAccess;
using DustIntheWind.EfCoreMigrationsDemo.Domain;

namespace DustIntheWind.EfCoreMigrationsDemo.Business;

internal class CreateOrderUseCase
{
    private readonly DemoDbContext demoDbContext;

    public CreateOrderUseCase(DemoDbContext demoDbContext)
    {
        this.demoDbContext = demoDbContext ?? throw new ArgumentNullException(nameof(demoDbContext));
    }

    public Task ExecuteAsync()
    {
        Customer customer = new()
        {
            Name = "John Doe"
        };

        Order order = new()
        {
            Date = DateTime.UtcNow,
            Customer = customer
        };

        _ = demoDbContext.Orders.Add(order);

        return demoDbContext.SaveChangesAsync();
    }
}
