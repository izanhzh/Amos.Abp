using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Amos.Abp.EntityFrameworkCore
{
    public abstract class AmosAbpDbContext<TDbContext> : AbpDbContext<TDbContext> where TDbContext : DbContext, IEfCoreDbContext
    {
        protected AmosAbpDbContext(DbContextOptions<TDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AutoAddEntityTypeToModel<TDbContext>();
            modelBuilder.AutoAddTempTableToModel<TDbContext>();

            //注意：必须要在base.OnModelCreating之前添加到Model，否则这些实体的自动软删除过滤将会失效，base.OnModelCreating中会调用modelBuilder.Model.GetEntityTypes对实体添加各种过滤器
            base.OnModelCreating(modelBuilder);
        }
    }
}
