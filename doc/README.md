# EF Core Migrations Tutorial

## Step 1 - Create a Console Application

Nothing special here, just create a new Console application.

## Step 2 - Install the Tools (for Visual Studio)

Globally install the Entity Framework Tools. This step must be done once.

```cmd
dotnet tool install --global dotnet-ef
```

Add design-time support

```powershell
Install-Package Microsoft.EntityFrameworkCore.Design
```

## Step 3 - Configure Microsoft Dependency Injection

```powershell
Install-Package Microsoft.EntityFrameworkCore.SqlServer
```

Use the `AddDbContext` method to configure the DB Context:

```
serviceCollection.AddDbContext<DemoDbContext>(optionsBuilder =>
{
    string connectionString = "...";
    optionsBuilder.UseSqlServer(connectionString);
});
```

## Step 4 - Create the Initial Migration

Create some entities:

```C#
internal class Customer
{
    ...
}

internal class Order
{
    ...
}
```

Create the DB Context:

```c#
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
```

The `CustomerTypeConfiguration` and `OrderTypeConfiguration` classes contain unintrusive instructions for mapping the entity to the database tables.

### Create the migration

A migration is a class containing code that updates the database. The following command will generate a new migration called `InitialCreate`. 

EF Migrations is compare the model currently existing in C# with the existing tables in the database, and will generate a new migration with instructions that, will bring the database in sync with the model. The database is not yet modified at this stage.

```powershell
dotnet ef migrations add InitialCreate
```

### Execute the migration

The following command will execute the migration and apply the changes in the database.

```powershell
dotnet ef database update
```

