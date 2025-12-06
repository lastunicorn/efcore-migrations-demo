using DustIntheWind.EfCoreMigrationsDemo.Domain;
using Microsoft.EntityFrameworkCore;

namespace DustIntheWind.EfCoreMigrationsDemo.DataAccess;

internal class DemoDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DemoDbContext(DbContextOptions<DemoDbContext> options)
        : base(options)
    {
    }
}
