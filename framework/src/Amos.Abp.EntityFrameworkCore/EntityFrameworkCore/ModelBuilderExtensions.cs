using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Amos.Abp.EntityFrameworkCore
{
    public static class ModelBuilderExtensions
    {
        public static void ConfigureAutoAddEntityTypes<TDbContext>(this ModelBuilder builder, AbpModelBuilderConfigurationOptions options) where TDbContext : IEfCoreDbContext
        {
            var entityTypes = EntityFinder.GetAutoAddEntityTypes(typeof(TDbContext));
            foreach (var entityType in entityTypes)
            {
                builder.Entity(entityType, (b) =>
                {
                    b.ToTable(options.TablePrefix + entityType.Name, options.Schema);
                    b.ConfigureByConvention();
                });
            }
        }
    }
}
