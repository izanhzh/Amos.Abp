using Volo.Abp.BackgroundWorkers.Quartz;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Amos.AbpLearn.ProductManagement
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(ProductManagementDomainSharedModule)
    )]
    public class ProductManagementDomainModule : AbpModule
    {

    }
}
