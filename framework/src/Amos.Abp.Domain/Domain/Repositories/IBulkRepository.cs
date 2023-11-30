using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Amos.Abp.Domain.Repositories
{
    /// <summary>
    ///  注意：暂时不支持审计字段自动赋值，软删除等
    ///  TODO：实现审计字段自动赋值，软删除等
    /// </summary>
    public interface IBulkRepository : IRepository
    {
        int BatchDelete(IQueryable query);

        Task<int> BatchDeleteAsync(IQueryable query, CancellationToken cancellationToken = default);

        int BatchUpdate(IQueryable query, object updateValues, List<string> updateColumns = null);

        int BatchUpdate<TEntity>(IQueryable<TEntity> query, Expression<Func<TEntity, TEntity>> updateExpression) where TEntity : class, IEntity;

        Task<int> BatchUpdateAsync(IQueryable query, object updateValues, List<string> updateColumns = null, CancellationToken cancellationToken = default);

        Task<int> BatchUpdateAsync<TEntity>(IQueryable<TEntity> query, Expression<Func<TEntity, TEntity>> updateExpression, CancellationToken cancellationToken = default) where TEntity : class, IEntity;

        void BulkDelete<TEntity>(IList<TEntity> entities) where TEntity : class, IEntity;

        Task BulkDeleteAsync<TEntity>(IList<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class, IEntity;

        void BulkInsert<TEntity>(IList<TEntity> entities) where TEntity : class, IEntity;

        Task BulkInsertAsync<TEntity>(IList<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class, IEntity;

        void BulkUpdate<TEntity>(IList<TEntity> entities) where TEntity : class, IEntity;

        Task BulkUpdateAsync<TEntity>(IList<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class, IEntity;

        void Truncate<TEntity>() where TEntity : class, IEntity;

        Task TruncateAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : class, IEntity;
    }
}
