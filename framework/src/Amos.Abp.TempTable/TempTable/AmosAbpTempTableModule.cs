using Amos.Abp.Domain;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Amos.Abp.TempTable
{
    [DependsOn(typeof(AmosAbpDomainModule))]
    [DependsOn(typeof(AbpEntityFrameworkCoreModule))]
    public class AmosAbpTempTableModule: AbpModule
    {
    }
}
