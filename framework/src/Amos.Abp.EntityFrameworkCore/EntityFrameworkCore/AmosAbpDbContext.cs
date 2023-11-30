using Amos.Abp.TempTable.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Amos.Abp.EntityFrameworkCore
{
    public abstract class AmosAbpDbContext<TDbContext> : AbpDbContext<TDbContext> where TDbContext : DbContext
    {
        protected AmosAbpDbContext(DbContextOptions<TDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
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
            var tempTables = TempTableFinder.GetAutoAddTempTables(typeof(TDbContext));
            foreach (var tempTable in tempTables)
            {
                var isExists = modelBuilder.Model.FindEntityType(tempTable) != null;
                if (!isExists)
                {
                    modelBuilder.Model.AddEntityType(tempTable);
                    modelBuilder.Entity(tempTable, (b) =>
                    {
                        //b.ToView(null);
                        b.ToTable("#TempTable_" + tempTable.Name, (t) => t.ExcludeFromMigrations());
                        b.HasNoKey();
                        b.ConfigureByConvention();
                    });
                }
            }
            //注意：必须要在base.OnModelCreating之前添加到Model，否则这些实体的自动软删除过滤将会失效，base.OnModelCreating中会调用modelBuilder.Model.GetEntityTypes对实体添加各种过滤器
            base.OnModelCreating(modelBuilder);
        }
    }
}
