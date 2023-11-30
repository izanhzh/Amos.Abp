using System.Threading.Tasks;

namespace Amos.Abp.SqlScript
{
    public interface ISqlScriptProvider
    {
        Task<string> GetSqlScriptAsync(string databaseProvider, string sqlScriptKey, object scriptRenderParam);
    }
}
