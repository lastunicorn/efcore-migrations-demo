using DustIntheWind.EfCoreMigrationsDemo.DataAccess;
using DustIntheWind.EfCoreMigrationsDemo.Domain;
using Microsoft.EntityFrameworkCore;

namespace DustIntheWind.EfCoreMigrationsDemo.Business;

internal class GetOrdersUseCase
{
    private readonly DemoDbContext demoDbContext;

    public GetOrdersUseCase(DemoDbContext demoDbContext)
    {
        this.demoDbContext = demoDbContext ?? throw new ArgumentNullException(nameof(demoDbContext));
    }

    internal async Task<List<Order>> ExecuteAsync()
    {
        return demoDbContext.Orders
            .Include(x => x.Customer)
            .OrderByDescending(x => x.Date)
            .ToList();
    }
}
