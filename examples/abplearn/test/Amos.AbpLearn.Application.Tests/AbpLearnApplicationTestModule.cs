using Volo.Abp.Modularity;

namespace Amos.AbpLearn
{
    [DependsOn(
        typeof(AbpLearnApplicationModule),
        typeof(AbpLearnDomainTestModule)
        )]
    public class AbpLearnApplicationTestModule : AbpModule
    {

    }
}