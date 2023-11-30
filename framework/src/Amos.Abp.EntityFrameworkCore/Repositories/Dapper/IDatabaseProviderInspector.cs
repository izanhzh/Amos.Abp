using System.Data;

namespace Amos.Abp.Repositories.Dapper
{
    public interface IDatabaseProviderInspector
    {
        string GetDatabaseProvider(IDbConnection dbConnection);
    }
}
