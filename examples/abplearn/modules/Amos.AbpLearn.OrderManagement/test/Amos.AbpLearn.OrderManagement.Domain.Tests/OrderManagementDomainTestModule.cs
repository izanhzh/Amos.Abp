using Amos.AbpLearn.OrderManagement.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Amos.AbpLearn.OrderManagement
{
    /* Domain tests are configured to use the EF Core provider.
     * You can switch to MongoDB, however your domain tests should be
     * database independent anyway.
     */
    [DependsOn(
        typeof(OrderManagementEntityFrameworkCoreTestModule)
        )]
    public class OrderManagementDomainTestModule : AbpModule
    {
        
    }
}
