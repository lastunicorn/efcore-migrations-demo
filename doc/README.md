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

## Step 3 - Configure Dependency Injection

### a) Add Microsoft Dependency Injection

```powershell
Install-Package Microsoft.Extensions.DependencyInjection
```

```c#
ServiceCollection serviceCollection = new();

// ... Configure services here
    
IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
```

### b) Configure the DB Context service

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

## Step 4 - Create the Initial Migration

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

### c) Create the migration

A migration is a class containing code that updates the structure of the database. The following command will generate a new migration called `InitialCreate` which will create tables for the new entities. 

EF Migrations compares the model currently existing in C# with the existing tables in the database, and will generate a new migration with instructions that, will bring the database in sync with the model.

```powershell
dotnet ef migrations add InitialCreate
```

> **Note**
>
> The database is not yet modified at this stage.

### d) Execute the migration

The following command will execute the migration and apply the changes in the database.

```powershell
dotnet ef database update
```

> **Note**
>
> The migration is executed only once. After the migration is applied, a note is added in a special table in the database (`__EFMigrationsHistory`) containing the name of the applied migration, so that it is not applied again next time the database is updated.