using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;

namespace Amos.Abp.TempTable.EntityFrameworkCore
{
    /// <summary>
    /// Extension temp table operation methods for <see cref="DbContext"/>.
    /// </summary>
    public static class DbContextTempTableExtensions
    {
        /// <summary>
        /// Copies <paramref name="entities"/> into a temp table and returns the query for accessing the inserted records.
        /// </summary>
        /// <param name="ctx">Database context.</param>
        /// <param name="entities">Entities to insert.</param>
        /// <param name="options">Options.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <typeparam name="TTempTable">Entity type.</typeparam>
        /// <returns>A query for accessing the inserted values.</returns>
        /// <exception cref="ArgumentNullException"> <paramref name="ctx"/> or <paramref name="entities"/> is <c>null</c>.</exception>
        public static async Task<IQueryable<TTempTable>> InsertIntoTempTableAsync<TDbContext, TTempTable>(this TDbContext ctx, IEnumerable<TTempTable> entities, TempTableInsertOptions options = null, CancellationToken cancellationToken = default)
            where TTempTable : class, ITempTable
            where TDbContext : IEfCoreDbContext
        {
            if (ctx == null)
                throw new ArgumentNullException(nameof(ctx));
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            options ??= new TempTableInsertOptions();
            var entityType = ctx.Model.FindEntityType(typeof(TTempTable));
            var tempTableCreator = ctx.GetService<ITempTableCreator>();
            var tempTableBulkExecutor = ctx.GetService<ITempTableBulkExecutor>();

            var tableName = await tempTableCreator.CreateTempTableAsync(ctx, entityType, options.TempTableCreationOptions, cancellationToken).ConfigureAwait(false);

            if (options.PrimaryKeyCreation == TempTablePrimaryKeyCreation.BeforeBulkInsert)
                await tempTableCreator.CreatePrimaryKeyAsync(ctx, entityType, tableName, !options.TempTableCreationOptions.MakeTableNameUnique, cancellationToken).ConfigureAwait(false);

            await tempTableBulkExecutor.BulkInsertAsync(ctx, entityType, entities, null, tableName, options.TempTableBulkExecutorOptions, cancellationToken).ConfigureAwait(false);

            if (options.PrimaryKeyCreation == TempTablePrimaryKeyCreation.AfterBulkInsert)
                await tempTableCreator.CreatePrimaryKeyAsync(ctx, entityType, tableName, !options.TempTableCreationOptions.MakeTableNameUnique, cancellationToken).ConfigureAwait(false);

            return ctx.GetTempTableQuery<TDbContext, TTempTable>(tableName);
        }

        private static IQueryable<TTempTable> GetTempTableQuery<TDbContext, TTempTable>(this TDbContext ctx, string tableName)
            where TDbContext : IEfCoreDbContext
            where TTempTable : class, ITempTable
        {
            var sql = $"SELECT * FROM [{tableName}]";
            return ctx.Set<TTempTable>().FromSqlRaw(sql);
        }
    }
}
