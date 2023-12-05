using Amos.AbpLearn.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Amos.AbpLearn
{
    [DependsOn(
        typeof(AbpLearnEntityFrameworkCoreTestModule)
        )]
    public class AbpLearnDomainTestModule : AbpModule
    {

    }
}