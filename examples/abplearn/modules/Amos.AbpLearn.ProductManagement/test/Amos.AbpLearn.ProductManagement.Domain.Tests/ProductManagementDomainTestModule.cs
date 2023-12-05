using Amos.AbpLearn.ProductManagement.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Amos.AbpLearn.ProductManagement
{
    /* Domain tests are configured to use the EF Core provider.
     * You can switch to MongoDB, however your domain tests should be
     * database independent anyway.
     */
    [DependsOn(
        typeof(ProductManagementEntityFrameworkCoreTestModule)
        )]
    public class ProductManagementDomainTestModule : AbpModule
    {
        
    }
}
