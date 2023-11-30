using Amos.Abp.Domain;
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
