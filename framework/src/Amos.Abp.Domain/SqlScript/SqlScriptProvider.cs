using Scriban;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Amos.Abp.SqlScript
{
    public class SqlScriptProvider : ISqlScriptProvider, ISingletonDependency
    {
        private readonly ISqlScriptStore _sqlScriptStore;

        public SqlScriptProvider(ISqlScriptStore sqlScriptStore)
        {
            _sqlScriptStore = sqlScriptStore;
        }

        public async Task<string> GetSqlScriptAsync(string databaseProvider, string sqlScriptKey, object scriptRenderParam)
        {
            var strs = sqlScriptKey.Split(':');
            if (strs.Length != 2)
            {
                throw new ArgumentException($"{nameof(sqlScriptKey)} non-compliance (<script-namespace>:<script-id>)");
            }
            var sqlScriptNamespace = strs[0];
            var sql = await _sqlScriptStore.GetSqlScriptAsync(sqlScriptNamespace, databaseProvider, sqlScriptKey);
            if (scriptRenderParam != null)
            {
                var template = Template.Parse(sql);
                sql = await template.RenderAsync(scriptRenderParam);
            }
            return sql;
        }
    }
}
