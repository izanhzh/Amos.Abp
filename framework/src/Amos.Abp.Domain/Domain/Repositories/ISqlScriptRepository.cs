using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Amos.Abp.Domain.Repositories
{
    /// <summary>
    /// SqlScriptKey格式约定：<script-namespace>:<script-id>
    /// </summary>
    public interface ISqlScriptRepository : IRepository
    {
        Task<string> GetSqlScriptAsync(string sqlScriptKey, object scriptRenderParam = null);

        Task<int> ExecuteAsync(string sqlScriptKey, object scriptRenderParam = null, object sqlParam = null);

        Task<object> ExecuteScalar(string sqlScriptKey, object scriptRenderParam = null, object sqlParam = null);

        Task<DataTable> QueryAsDataTableAsync(string sqlScriptKey, object scriptRenderParam = null, object sqlParam = null);

        Task<DataSet> QueryAsDataSetAsync(string sqlScriptKey, object scriptRenderParam = null, object sqlParam = null);

        Task<IEnumerable<T>> QueryAsync<T>(string sqlScriptKey, object scriptRenderParam = null, object sqlParam = null);
    }
}
