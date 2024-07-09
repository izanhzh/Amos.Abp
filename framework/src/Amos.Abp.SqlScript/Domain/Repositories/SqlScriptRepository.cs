using Amos.Abp.Domain.Repositories;
using Amos.Abp.SqlScript;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.Dapper;
using Volo.Abp.EntityFrameworkCore;

namespace Amos.Abp.Repositories
{
    public abstract class SqlScriptRepository<TDbContext> : SqlScriptRepository<TDbContext, TDbContext>, ISqlScriptRepository
       where TDbContext : DbContext, IEfCoreDbContext
    {
        protected SqlScriptRepository(IDbContextProvider<TDbContext> dbContextProvider, ISqlScriptProvider sqlScriptProvider)
            : base(dbContextProvider, sqlScriptProvider)
        {
        }
    }

    public abstract class SqlScriptRepository<TDbContext, IDbContext> : DapperRepository<IDbContext>, ISqlScriptRepository
    where TDbContext : DbContext, IDbContext
    where IDbContext : IEfCoreDbContext
    {
        private readonly IDbContextProvider<IDbContext> _dbContextProvider;
        private readonly ISqlScriptProvider _sqlScriptProvider;

        public SqlScriptRepository(
            IDbContextProvider<IDbContext> dbContextProvider,
            ISqlScriptProvider sqlScriptProvider) : base(dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
            _sqlScriptProvider = sqlScriptProvider;
        }

        public async Task<string> GetSqlScriptAsync(string sqlScriptKey, object scriptRenderParam = null)
        {
            var databaseProvider = await GetDatabaseProviderAsync();
            var sql = await _sqlScriptProvider.GetSqlScriptAsync(databaseProvider, sqlScriptKey, scriptRenderParam);
            return sql;
        }

        public virtual async Task<int> ExecuteAsync(string sqlScriptKey, object scriptRenderParam = null, object sqlParam = null)
        {
            var dbConnection = await GetDbConnectionAsync();
            var dbTransaction = await GetDbTransactionAsync();

            var sql = await GetSqlScriptAsync(sqlScriptKey, scriptRenderParam);

            return await dbConnection.ExecuteAsync(sql, sqlParam, transaction: dbTransaction);
        }

        public async Task<object> ExecuteScalar(string sqlScriptKey, object scriptRenderParam = null, object sqlParam = null)
        {
            var dbConnection = await GetDbConnectionAsync();
            var dbTransaction = await GetDbTransactionAsync();

            var sql = await GetSqlScriptAsync(sqlScriptKey, scriptRenderParam);

            return await dbConnection.ExecuteScalarAsync(sql, sqlParam, transaction: dbTransaction);
        }

        public virtual async Task<DataTable> QueryAsDataTableAsync(string sqlScriptKey, object scriptRenderParam = null, object sqlParam = null)
        {
            var dbConnection = await GetDbConnectionAsync();
            var dbTransaction = await GetDbTransactionAsync();

            var sql = await GetSqlScriptAsync(sqlScriptKey, scriptRenderParam);

            using var reader = await dbConnection.ExecuteReaderAsync(sql, sqlParam, transaction: dbTransaction);

            var table = new DataTable();
            table.Load(reader);
            return table;
        }

        public virtual async Task<DataSet> QueryAsDataSetAsync(string sqlScriptKey, object scriptRenderParam = null, object sqlParam = null)
        {
            var dbConnection = await GetDbConnectionAsync();
            var dbTransaction = await GetDbTransactionAsync();

            var sql = await GetSqlScriptAsync(sqlScriptKey, scriptRenderParam);

            using var reader = await dbConnection.ExecuteReaderAsync(sql, sqlParam, transaction: dbTransaction);

            var ds = ConvertDataReaderToDataSet(reader);
            return ds;
        }

        public virtual async Task<IEnumerable<T>> QueryAsync<T>(string sqlScriptKey, object scriptRenderParam = null, object sqlParam = null)
        {
            var dbConnection = await GetDbConnectionAsync();
            var dbTransaction = await GetDbTransactionAsync();

            var sql = await GetSqlScriptAsync(sqlScriptKey, scriptRenderParam);

            return await dbConnection.QueryAsync<T>(sql, sqlParam, transaction: dbTransaction);
        }

        private async Task<string> GetDatabaseProviderAsync()
        {
            var dbContext = await _dbContextProvider.GetDbContextAsync();
            var databaseProvider = (EfCoreDatabaseProvider)((TDbContext)dbContext).Model["_Abp_DatabaseProvider"];
            return databaseProvider.ToString();
        }

        private DataSet ConvertDataReaderToDataSet(IDataReader reader)
        {
            DataSet dataSet = new DataSet();
            do
            {
                // Create new data table
                DataTable schemaTable = reader.GetSchemaTable();
                DataTable dataTable = new DataTable();
                if (schemaTable != null)
                {
                    // A query returning records was executed 
                    for (int i = 0; i < schemaTable.Rows.Count; i++)
                    {
                        DataRow dataRow = schemaTable.Rows[i];
                        // Create a column name that is unique in the data table 
                        string columnName = (string)dataRow["ColumnName"]; //+ " // Add the column definition to the data table 
                        DataColumn column = new DataColumn(columnName, (Type)dataRow["DataType"]);
                        dataTable.Columns.Add(column);
                    }
                    dataSet.Tables.Add(dataTable);
                    // Fill the data table we just created
                    while (reader.Read())
                    {
                        DataRow dataRow = dataTable.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            dataRow[i] = reader.GetValue(i);
                        }
                        dataTable.Rows.Add(dataRow);
                    }
                }
                else
                {
                    // No records were returned
                    DataColumn column = new DataColumn("RowsAffected");
                    dataTable.Columns.Add(column);
                    dataSet.Tables.Add(dataTable);
                    DataRow dataRow = dataTable.NewRow();
                    dataRow[0] = reader.RecordsAffected;
                    dataTable.Rows.Add(dataRow);
                }
            }
            while (reader.NextResult());
            return dataSet;

        }
    }
}
