using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Amos.AbpLearn.OrderManagement
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(OrderManagementDomainSharedModule)
    )]
    public class OrderManagementDomainModule : AbpModule
    {

    }
}
