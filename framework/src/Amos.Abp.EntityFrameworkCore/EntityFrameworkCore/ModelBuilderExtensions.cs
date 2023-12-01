using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Amos.Abp.EntityFrameworkCore
{
    public static class ModelBuilderExtensions
    {
        public static void AutoAddEntityTypeToModel<TDbContext>(this ModelBuilder modelBuilder) where TDbContext : IEfCoreDbContext
        {
            var entityTypes = EntityFinder.GetAutoAddEntityTypes(typeof(TDbContext));
            foreach (var entityType in entityTypes)
            {
                var isExists = modelBuilder.Model.FindEntityType(entityType) != null;
                if (!isExists)
                {
                    modelBuilder.Model.AddEntityType(entityType);
                }
            }
        }

        public static void ConfigureAutoAddEntityTypes<TDbContext>(this ModelBuilder modelBuilder, AbpModelBuilderConfigurationOptions options) where TDbContext : IEfCoreDbContext
        {
            var entityTypes = EntityFinder.GetAutoAddEntityTypes(typeof(TDbContext));
            foreach (var entityType in entityTypes)
            {
                modelBuilder.Entity(entityType, (b) =>
                {
                    b.ToTable(options.TablePrefix + entityType.Name, options.Schema);
                    b.ConfigureByConvention();
                });
            }
        }
    }
}
