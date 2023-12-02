# Amos.Abp
The repository is an extension based on Volo Abp(https://github.com/abpframework/abp)

## Automatically add entities to the DbContext Model
1. As normal, create your Entity class in the Domain layer
2. Add the `Amos.Abp.EntityFrameworkCore` Nuget package to your EntityFrameworkCore layer
3. Using `AutoAddEntityToModelAttribute` tag your DbContext interface
```C#
[AutoAddEntityToModel(typeof(YourDomainModule))]
public interface IYourDbContext : IEfCoreDbContext
{
}
```
4. Call the `AutoAddEntityTypeToModel` method in your DbContext class (Note: Call it before base.OnModelCreating)
```C#
[ConnectionStringName(YourDbProperties.ConnectionStringName)]
public class YourDbContext : AbpDbContext<YourDbContext>, IYourDbContext
{
  public YourDbContext(DbContextOptions<YourDbContext> options): base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder builder)
  {
      //Note: Call it before base.OnModelCreating
      builder.AutoAddEntityTypeToModel(this);
      base.OnModelCreating(builder);
      builder.ConfigureYourModule();
  }
}
```
5. Follow Volo abp modular best practices, configure the automatically add entities in your ModuleDbContextModelCreatingExtensions
```C#
public static class YourModuleDbContextModelCreatingExtensions
{
    public static void ConfigureYourModule(
        this ModelBuilder builder,
        Action<YourModuleModelBuilderConfigurationOptions> optionsAction = null)
    {
        Check.NotNull(builder, nameof(builder));

        var options = new YourModuleModelBuilderConfigurationOptions(
            YourDbProperties.DbTablePrefix,
            YourDbProperties.DbSchema
        );

        optionsAction?.Invoke(options);

        builder.ConfigureAutoAddEntityTypes<IYourDbContext>((entityType) => (b) =>
        {
            b.ToTable(options.TablePrefix + entityType.Name, options.Schema);
            b.ConfigureByConvention();
        });

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
```
You are advised to configure the index, Key, and so on for the Entity through the IEntityTypeConfiguration interface that inherits EfCore, at your EntityFrameworkCore layer
```C#
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasIndex(h => h.Name).HasFilter("IsDeleted=0").IsUnique();
    }
}
```
Then by calling the `builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly())`  configuration entity, just like the example above
6. Register automatically add entities repository in your EntityFrameworkCoreModule, use `AddAbpDbContextEx` instead of `AddAbpDbContext`
```C#
[DependsOn(typeof(YourDomainModule))]
[DependsOn(typeof(AmosAbpEntityFrameworkCoreModule))]
public class YourEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContextEx<OrderManagementDbContext>(options =>{});
    }
}
```
Note: Add DependsOn
7. Finally, as normal, you can inject automatically add entity repository to use
```C#
public class YourAppService : IYourAppService
{
    private readonly Lazy<IRepository<XXEntity, long>> _xxRepositoryLazy;

    public YourAppService(Lazy<IRepository<XXEntity, long>> xxRepositoryLazy)
    {
        _xxRepositoryLazy = xxRepositoryLazy;
    }
}
```

## Temp table
TODO: description document

## SqlScript
TODO: description document

## BulkRepository
TODO: description document
