using DustIntheWind.EfCoreMigrationsDemo.DataAccess.EntityConfigurations;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CustomerTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderTypeConfiguration());
    }
}
