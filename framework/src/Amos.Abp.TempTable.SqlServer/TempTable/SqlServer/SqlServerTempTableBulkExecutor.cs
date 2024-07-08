using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace Amos.Abp.TempTable.SqlServer
{
    public class SqlServerTempTableBulkExecutor : ITempTableBulkExecutor, ISingletonDependency
    {
        public Task BulkInsertAsync<TDbContext, TTempTable>(
            TDbContext ctx,
            IEntityType entityType,
            IEnumerable<TTempTable> entities,
            TempTableBulkExecutorOptions options,
            CancellationToken cancellationToken = default)
            where TDbContext : DbContext
            where TTempTable : class, ITempTable
        {
            if (entityType == null)
                throw new ArgumentNullException(nameof(entityType));

            return BulkInsertAsync(ctx, entityType, entities, entityType.GetSchema(), entityType.GetTableName(), options, cancellationToken);
        }

        public async Task BulkInsertAsync<TDbContext, TTempTable>(
            TDbContext ctx,
            IEntityType entityType,
            IEnumerable<TTempTable> entities,
            string schema,
            string tableName,
            TempTableBulkExecutorOptions options,
            CancellationToken cancellationToken = default)
            where TDbContext : DbContext
            where TTempTable : class, ITempTable
        {
            if (ctx == null)
                throw new ArgumentNullException(nameof(ctx));
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            if (tableName == null)
                throw new ArgumentNullException(nameof(tableName));
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            var factory = ctx.GetService<IEntityDataReaderFactory>();
            var properties = GetPropertiesForInsert(options.EntityMembersProvider, entityType);
            var sqlCon = (SqlConnection)ctx.Database.GetDbConnection();
            var sqlTx = (SqlTransaction)ctx.Database.CurrentTransaction?.GetDbTransaction();
            var storeObjectIdentifier = StoreObjectIdentifier.SqlQuery(entityType);

            using (var reader = factory.Create(entities, properties))
            using (var bulkCopy = new SqlBulkCopy(sqlCon, options.SqlBulkCopyOptions, sqlTx))
            {
                bulkCopy.DestinationTableName = $"[{tableName}]";

                if (!String.IsNullOrWhiteSpace(schema))
                    bulkCopy.DestinationTableName = $"[{schema}].{bulkCopy.DestinationTableName}";

                bulkCopy.EnableStreaming = options.EnableStreaming;

                if (options.BulkCopyTimeout.HasValue)
                    bulkCopy.BulkCopyTimeout = (int)options.BulkCopyTimeout.Value.TotalSeconds;

                if (options.BatchSize.HasValue)
                    bulkCopy.BatchSize = options.BatchSize.Value;

                foreach (var property in reader.Properties)
                {
                    var index = reader.GetPropertyIndex(property);
                    bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(index, property.GetColumnName(storeObjectIdentifier)));
                }

                await ctx.Database.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);

                await bulkCopy.WriteToServerAsync(reader, cancellationToken).ConfigureAwait(false);
            }
        }

        private static IList<IProperty> GetPropertiesForInsert(IEntityMembersProvider entityMembersProvider, IEntityType entityType)
        {
            if (entityMembersProvider == null)
                return entityType.GetProperties().Where(p => !p.IsShadowProperty() && p.GetBeforeSaveBehavior() != PropertySaveBehavior.Ignore).ToList();

            return ConvertToEntityProperties(entityMembersProvider.GetMembers(), entityType);
        }

        private static IList<IProperty> ConvertToEntityProperties(IReadOnlyList<MemberInfo> memberInfos, IEntityType entityType)
        {
            var properties = new IProperty[memberInfos.Count];

            for (var i = 0; i < memberInfos.Count; i++)
            {
                var memberInfo = memberInfos[i];
                var property = FindProperty(entityType, memberInfo);

                properties[i] = property ?? throw new ArgumentException($"The member '{memberInfo.Name}' has not found on entity '{entityType.Name}'.", nameof(memberInfos));
            }

            return properties;
        }

        private static IProperty FindProperty(IEntityType entityType, MemberInfo memberInfo)
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.PropertyInfo == memberInfo || property.FieldInfo == memberInfo)
                    return property;
            }

            return null;
        }
    }
}
