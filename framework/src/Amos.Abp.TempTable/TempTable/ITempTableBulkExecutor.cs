﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;

namespace Amos.Abp.TempTable
{
    public interface ITempTableBulkExecutor
    {
        Task BulkInsertAsync<TDbContext, TTempTable>(
            TDbContext ctx,
            IEntityType entityType,
            IEnumerable<TTempTable> entities,
            TempTableBulkExecutorOptions options,
            CancellationToken cancellationToken = default)
            where TDbContext : DbContext
            where TTempTable : class, ITempTable;

        Task BulkInsertAsync<TDbContext, TTempTable>(
            TDbContext ctx,
            IEntityType entityType,
            IEnumerable<TTempTable> entities,
            string schema,
            string tableName,
            TempTableBulkExecutorOptions options,
            CancellationToken cancellationToken = default)
            where TDbContext : DbContext
            where TTempTable : class, ITempTable;
    }
}
