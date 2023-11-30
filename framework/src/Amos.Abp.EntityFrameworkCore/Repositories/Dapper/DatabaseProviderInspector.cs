using System;
using System.Data;
using Volo.Abp.DependencyInjection;

namespace Amos.Abp.Repositories.Dapper
{
    public class DatabaseProviderInspector : IDatabaseProviderInspector, ISingletonDependency
    {
        public string GetDatabaseProvider(IDbConnection dbConnection)
        {
            return dbConnection.GetType().Name switch
            {
                "SqlConnection" => "SqlServer",
                _ => throw new NotSupportedException(),
            };
        }
    }
}
