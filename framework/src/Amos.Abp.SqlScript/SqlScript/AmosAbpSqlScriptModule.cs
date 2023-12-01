using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Dapper;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Amos.Abp.SqlScript
{
    [DependsOn(typeof(AbpDapperModule))]
    public class AmosAbpSqlScriptModule : AbpModule
    {
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            InitSqlScript(context);
        }

        private void InitSqlScript(ApplicationInitializationContext context)
        {
            var options = context
                .ServiceProvider
                .GetRequiredService<IOptions<SqlScriptOptions>>()
                .Value;

            var sqlScriptStore = context.ServiceProvider.GetRequiredService<ISqlScriptStore>();

            foreach (var entry in options.SqlScriptResources)
            {
                var sqlScriptNamespace = entry.Key;
                var sqlScriptResourceType = entry.Value;
                AsyncHelper.RunSync(() => sqlScriptStore.AddResourceAsync(sqlScriptNamespace, sqlScriptResourceType));
            }
        }
    }
}
