using Amos.Abp.Domain;
using Amos.Abp.TempTable;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Amos.Abp.EntityFrameworkCore
{
    [DependsOn(typeof(AmosAbpDomainModule))]
    [DependsOn(typeof(AbpEntityFrameworkCoreModule))]
    public class AmosAbpEntityFrameworkCoreModule : AbpModule
    {
    }
}
