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

This package is needed when running Migrations commands to generate migration scripts.

## Step 3 - Add a Dependency Injection Container

```powershell
Install-Package Microsoft.Extensions.DependencyInjection
```

Create a `ServiceCollection` used to configure the dependency container.

```c#
ServiceCollection serviceCollection = new();
Setup.ConfigureServices(serviceCollection);    
IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
```

Configure the services.

```c#
internal static class Setup
{
    public static void ConfigureServices(ServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContext<DemoDbContext>(o =>
        {
			// ... Configure services here
        });
    }
}
```

## Step 4 - Configure the DB Context service

```powershell
Install-Package Microsoft.EntityFrameworkCore
Install-Package Microsoft.EntityFrameworkCore.SqlServer
```

Use the `AddDbContext` method to configure the DB Context:

```c#
serviceCollection.AddDbContext<DemoDbContext>(optionsBuilder =>
{
    string connectionString = "...";
    optionsBuilder.UseSqlServer(connectionString);
});
```

> **Note**
>
> The connection string should be retrieved from the app's configuration. Do not hardcode it.
>
> The `DemoDbContext` class is created in the next step.

## Step 5 - Create the DbContext

### a) Create Entities

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

### b) Create the DB Context

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

See the demo project for examples.

## Step 6 - Create the Initial Migration

**Migration** - A migration is a class containing code that updates the structure of the database.

The following command will generate a new migration called `InitialCreate` which will create tables for the new entities. 

EF Migrations compares the model currently existing in C# with the existing tables in the database, and will generate a new migration with instructions that, will bring the database in sync with the model.

This command must be run in the project directory, not the solution directory.

```powershell
dotnet ef migrations add InitialCreate
```

> **Note**
>
> The database is not yet modified at this stage.

Only for a Console application (not for ASP.NET Core application) Add a factory class that instantiates the `DemoDbContext` to be used by EF Migrations during the migrations setup. EF Migrations must investigate what is the current structure of the DbContext and used models.

We may use the same `Setup.ConfigureServices()` to configure the services. This approach ensures that the setup is done as similar as possible to the production setup.

```c#
internal class DemoDbContextFactory : IDesignTimeDbContextFactory<DemoDbContext>
{
    public DemoDbContext CreateDbContext(string[] args)
    {
        ServiceCollection serviceCollection = new();
        Setup.ConfigureServices(serviceCollection);
        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        return serviceProvider.GetRequiredService<DemoDbContext>();
    }
}
```

## Step 7 - Execute the migration

The following command will execute the migration and apply the changes in the database.

```powershell
dotnet ef database update
```

> **Note**
>
> The migration is executed only once. After the migration is applied, a record is added in a special table in the database (`__EFMigrationsHistory`) containing the name of the applied migration, so that it is not applied again next time the database is updated.