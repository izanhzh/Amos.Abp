using Volo.Abp.Modularity;

namespace Amos.AbpLearn.ProductManagement
{
    [DependsOn(
        typeof(ProductManagementApplicationModule),
        typeof(ProductManagementDomainTestModule)
        )]
    public class ProductManagementApplicationTestModule : AbpModule
    {

    }
}
