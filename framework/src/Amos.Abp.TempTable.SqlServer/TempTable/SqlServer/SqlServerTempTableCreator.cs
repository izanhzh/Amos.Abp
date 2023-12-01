using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace Amos.Abp.TempTable.SqlServer
{
    public class SqlServerTempTableCreator : ITempTableCreator, ISingletonDependency
    {
        public async Task<string> CreateTempTableAsync<TDbContext>(
            TDbContext ctx,
            IEntityType entityType,
            TempTableCreationOptions options,
            CancellationToken cancellationToken = default)
            where TDbContext : IEfCoreDbContext
        {
            if (ctx == null)
                throw new ArgumentNullException(nameof(ctx));
            if (entityType == null)
                throw new ArgumentNullException(nameof(entityType));
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            var tableName = entityType.GetTableName() ?? entityType.ShortName() ?? entityType.ClrType.Name;

            if (!tableName.StartsWith("#", StringComparison.Ordinal))
                tableName = $"#{tableName}";

            if (options.MakeTableNameUnique)
                tableName = $"{tableName}_{Guid.NewGuid():N}";

            var properties = entityType.GetProperties();
            var storeObjectIdentifier = StoreObjectIdentifier.SqlQuery(entityType);
            var sql = GetTempTableCreationSql(properties, tableName, storeObjectIdentifier, options.MakeTableNameUnique);

            await ctx.Database.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);

            await ctx.Database.ExecuteSqlRawAsync(sql, cancellationToken).ConfigureAwait(false);

            return tableName;
        }

        public async Task CreatePrimaryKeyAsync<TDbContext>(
            TDbContext ctx,
            IEntityType entityType,
            string tableName,
            bool checkForExistence = false,
            CancellationToken cancellationToken = default)
            where TDbContext : IEfCoreDbContext
        {
            if (ctx == null)
                throw new ArgumentNullException(nameof(ctx));
            if (entityType == null)
                throw new ArgumentNullException(nameof(entityType));
            if (tableName == null)
                throw new ArgumentNullException(nameof(tableName));

            IEnumerable<string> columnNames;

            var pk = entityType.FindPrimaryKey();

            //columnNames = pk.Properties.Select(p => p.GetColumnBaseName());
            var storeObjectIdentifier = StoreObjectIdentifier.SqlQuery(entityType);
            columnNames = pk.Properties.Select(p => p.GetColumnName(storeObjectIdentifier));

            var sql = $@"
ALTER TABLE [{tableName}]
ADD CONSTRAINT [PK_{tableName}_{Guid.NewGuid():N}] PRIMARY KEY CLUSTERED ({String.Join(", ", columnNames)});
";

            if (checkForExistence)
            {
                sql = $@"
IF(NOT EXISTS (SELECT * FROM tempdb.INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND OBJECT_ID(TABLE_CATALOG + '..' + TABLE_NAME) = OBJECT_ID('tempdb..{tableName}')))
BEGIN
{sql}
END
";
            }

            await ctx.Database.ExecuteSqlRawAsync(sql, cancellationToken).ConfigureAwait(false);
        }

        private static string GetTempTableCreationSql(IEnumerable<IProperty> properties, string tableName, StoreObjectIdentifier storeObjectIdentifier, bool isUnique)
        {
            if (properties == null)
                throw new ArgumentNullException(nameof(properties));
            if (tableName == null)
                throw new ArgumentNullException(nameof(tableName));

            var sql = $@"
      CREATE TABLE [{tableName}]
      (
{GetColumnsDefinitions(properties, storeObjectIdentifier)}
      );";

            if (isUnique)
                return sql;

            return $@"
IF(OBJECT_ID('tempdb..{tableName}') IS NOT NULL)
   TRUNCATE TABLE [{tableName}];
ELSE
BEGIN
{sql}
END
";
        }

        private static string GetColumnsDefinitions(IEnumerable<IProperty> properties, StoreObjectIdentifier storeObjectIdentifier)
        {
            if (properties == null)
                throw new ArgumentNullException(nameof(properties));

            var sb = new StringBuilder();
            var isFirst = true;

            foreach (var property in properties)
            {
                if (!isFirst)
                    sb.AppendLine(",");

                sb.Append("\t\t")
                  //.Append(property.GetColumnBaseName()).Append(" ")
                  .Append(property.GetColumnName(storeObjectIdentifier)).Append(" ")
                  .Append(property.GetColumnType()).Append(" ")
                  .Append(property.IsNullable ? "NULL" : "NOT NULL");

                isFirst = false;
            }

            return sb.ToString();
        }
    }
}
