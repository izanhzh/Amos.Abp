using EFCore.BulkExtensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Amos.Abp.Repositories.EntityFrameworkCore
{
    /// <summary>
    /// 注意：暂不支持对审计字段赋值，软删除等，暂时不提供自动注册IOC
    /// TODO：提供对审计字段赋值，软删除等支持
    /// </summary>
    public class AmosAbpEfCoreBulkOperationProvider : IEfCoreBulkOperationProvider
    {
        public async Task DeleteManyAsync<TDbContext, TEntity>(IEfCoreRepository<TEntity> repository, IEnumerable<TEntity> entities, bool autoSave, CancellationToken cancellationToken)
              where TDbContext : IEfCoreDbContext
              where TEntity : class, IEntity
        {
            var dbContext = await repository.GetDbContextAsync();
            await dbContext.BulkDeleteAsync(entities.ToList());
            if (autoSave)
            {
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task InsertManyAsync<TDbContext, TEntity>(IEfCoreRepository<TEntity> repository, IEnumerable<TEntity> entities, bool autoSave, CancellationToken cancellationToken)
            where TDbContext : IEfCoreDbContext
            where TEntity : class, IEntity
        {
            var dbContext = await repository.GetDbContextAsync();
            await dbContext.BulkInsertAsync(entities.ToList());
            if (autoSave)
            {
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateManyAsync<TDbContext, TEntity>(IEfCoreRepository<TEntity> repository, IEnumerable<TEntity> entities, bool autoSave, CancellationToken cancellationToken)
            where TDbContext : IEfCoreDbContext
            where TEntity : class, IEntity
        {
            var dbContext = await repository.GetDbContextAsync();
            await dbContext.BulkUpdateAsync(entities.ToList());
            if (autoSave)
            {
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
