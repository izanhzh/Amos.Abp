using Volo.Abp.Modularity;

namespace Amos.Abp.TempTable.SqlServer
{
    [DependsOn(typeof(AmosAbpTempTableModule))]
    public class AmosAbpTempTableSqlServerModule : AbpModule
    {
    }
}
