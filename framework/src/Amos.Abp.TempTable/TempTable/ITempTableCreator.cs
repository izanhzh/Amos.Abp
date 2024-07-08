using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;

namespace Amos.Abp.TempTable
{
    /// <summary>
    /// Creates temp tables.
    /// </summary>
    public interface ITempTableCreator
    {
        /// <summary>
        /// Creates a temp table.
        /// </summary>
        /// <param name="ctx">Database context to use.</param>
        /// <param name="entityType">Entity/query type.</param>
        /// <param name="options">Options.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Table name.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ctx"/> is <c>null</c>
        /// - or
        /// <paramref name="entityType"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">The provided type <paramref name="entityType"/> is not known by the <paramref name="ctx"/>.</exception>
        Task<string> CreateTempTableAsync<TDbContext>(TDbContext ctx, IEntityType entityType, TempTableCreationOptions options, CancellationToken cancellationToken = default) where TDbContext : DbContext;

        /// <summary>
        /// Creates a primary key in a temp table with provided <paramref name="tableName"/>.
        /// 
        /// <remarks>
        /// If the type <paramref name="entityType"/> is a query type then all columns are part of the primary key
        /// because a query type has no "Primary Key" by definition.
        /// </remarks>
        /// </summary>
        /// <param name="ctx">Database context to use.</param>
        /// <param name="entityType">Entity type.</param>
        /// <param name="tableName">Table name to create the primary key in.</param>
        /// <param name="checkForExistence">If <c>true</c> then the primary key is not going to be created if it exists already.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task CreatePrimaryKeyAsync<TDbContext>(TDbContext ctx, IEntityType entityType, string tableName, bool checkForExistence = false, CancellationToken cancellationToken = default) where TDbContext : DbContext;
    }
}
