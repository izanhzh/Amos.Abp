using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace Amos.AbpLearn.OrderManagement
{
    [DependsOn(
        typeof(OrderManagementDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class OrderManagementApplicationContractsModule : AbpModule
    {

    }
}
