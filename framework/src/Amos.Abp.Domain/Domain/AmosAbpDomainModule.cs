using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Amos.Abp.Domain
{
    [DependsOn(typeof(AbpDddDomainModule))]
    public class AmosAbpDomainModule : AbpModule
    {
    }
}
