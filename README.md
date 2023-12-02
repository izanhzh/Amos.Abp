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
5. Configure the automatically add entities in your ModuleDbContextModelCreatingExtensions
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

        builder.ConfigureAutoAddEntityTypes<IProductManagementDbContext>((entityType) => (b) =>
        {
            b.ToTable(options.TablePrefix + entityType.Name, options.Schema);
            b.ConfigureByConvention();
        });

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
```
 
TODO: description document

## Temp table
TODO: description document

## SqlScript
TODO: description document

## BulkRepository
TODO: description document
