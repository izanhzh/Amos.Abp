using System;
using System.Threading.Tasks;

namespace Amos.Abp.SqlScript
{
    public interface ISqlScriptStore
    {
        Task AddResourceAsync(string sqlScriptNamespace, Type sqlScriptResourceType);

        Task<string> GetSqlScriptAsync(string sqlScriptNamespace, string databaseProvider, string sqlScriptKey);
    }
}
