using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Volo.Abp.EntityFrameworkCore;

namespace Amos.Abp.EntityFrameworkCore
{
    public static class ModelBuilderExtensions
    {
        public static void AutoAddEntityTypeToModel<TDbContext>(this ModelBuilder modelBuilder, TDbContext _) where TDbContext : IEfCoreDbContext
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

        public static void ConfigureAutoAddEntityTypes<TDbContext>(this ModelBuilder modelBuilder, Action<EntityTypeBuilder> buildAction) where TDbContext : IEfCoreDbContext
        {
            var entityTypes = EntityFinder.GetAutoAddEntityTypes(typeof(TDbContext));
            foreach (var entityType in entityTypes)
            {
                modelBuilder.Entity(entityType, buildAction);
            }
        }
    }
}
