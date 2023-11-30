using Amos.Abp.Domain.Repositories;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Threading;

namespace Amos.Abp.Repositories.EntityFrameworkCore
{
    public abstract class EfCoreBulkRepository<TDbContext> : IBulkRepository where TDbContext : IEfCoreDbContext
    {
        private readonly IDbContextProvider<TDbContext> _dbContextProvider;

        protected EfCoreBulkRepository(IDbContextProvider<TDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public virtual int BatchDelete(IQueryable query)
        {
            return query.BatchDelete();
        }

        public virtual Task<int> BatchDeleteAsync(IQueryable query, CancellationToken cancellationToken = default)
        {
            return query.BatchDeleteAsync(cancellationToken);
        }

        public virtual int BatchUpdate(IQueryable query, object updateValues, List<string> updateColumns = null)
        {
            return query.BatchUpdate(updateValues, updateColumns);
        }

        public virtual int BatchUpdate<TEntity>(IQueryable<TEntity> query, Expression<Func<TEntity, TEntity>> updateExpression) where TEntity : class, IEntity
        {
            return query.BatchUpdate(updateExpression);
        }

        public virtual Task<int> BatchUpdateAsync(IQueryable query, object updateValues, List<string> updateColumns = null, CancellationToken cancellationToken = default)
        {
            return query.BatchUpdateAsync(updateValues, updateColumns, cancellationToken);
        }

        public virtual Task<int> BatchUpdateAsync<TEntity>(IQueryable<TEntity> query, Expression<Func<TEntity, TEntity>> updateExpression, CancellationToken cancellationToken) where TEntity : class, IEntity
        {
            return query.BatchUpdateAsync(updateExpression, null, cancellationToken);
        }

        public virtual void BulkDelete<TEntity>(IList<TEntity> entities) where TEntity : class, IEntity
        {
            var dbContext = GetDbContext();
            dbContext.BulkDelete(entities);
        }

        public virtual async Task BulkDeleteAsync<TEntity>(IList<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class, IEntity
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.BulkDeleteAsync(entities, cancellationToken: cancellationToken);
        }

        public virtual void BulkInsert<TEntity>(IList<TEntity> entities) where TEntity : class, IEntity
        {
            var dbContext = GetDbContext();
            dbContext.BulkInsert(entities);
        }

        public virtual async Task BulkInsertAsync<TEntity>(IList<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class, IEntity
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.BulkInsertAsync(entities, cancellationToken: cancellationToken);
        }

        public virtual void BulkUpdate<TEntity>(IList<TEntity> entities) where TEntity : class, IEntity
        {
            var dbContext = GetDbContext();
            dbContext.BulkUpdate(entities);
        }

        public virtual async Task BulkUpdateAsync<TEntity>(IList<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class, IEntity
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.BulkUpdateAsync(entities, cancellationToken: cancellationToken);
        }

        public virtual void Truncate<TEntity>() where TEntity : class, IEntity
        {
            var dbContext = GetDbContext();
            dbContext.Truncate<TEntity>();
        }

        public virtual async Task TruncateAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : class, IEntity
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.TruncateAsync<TEntity>(cancellationToken: cancellationToken);
        }

        protected virtual DbContext GetDbContext()
        {
            return AsyncHelper.RunSync(() => GetDbContextAsync());
        }

        protected virtual async Task<DbContext> GetDbContextAsync()
        {
            var dbContext = await _dbContextProvider.GetDbContextAsync();
            if (dbContext is DbContext result)
            {
                return result;
            }
            throw new NotSupportedException($"{dbContext.GetType().FullName} is not inherited DbContext");
        }
    }
}
