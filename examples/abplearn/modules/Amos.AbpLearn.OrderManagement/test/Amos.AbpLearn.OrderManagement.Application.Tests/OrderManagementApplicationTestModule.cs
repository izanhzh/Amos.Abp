using Volo.Abp.Modularity;

namespace Amos.AbpLearn.OrderManagement
{
    [DependsOn(
        typeof(OrderManagementApplicationModule),
        typeof(OrderManagementDomainTestModule)
        )]
    public class OrderManagementApplicationTestModule : AbpModule
    {

    }
}
