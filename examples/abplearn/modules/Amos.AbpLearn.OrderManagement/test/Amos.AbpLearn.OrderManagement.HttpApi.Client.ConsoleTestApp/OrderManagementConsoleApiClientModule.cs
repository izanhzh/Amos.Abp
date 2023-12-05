using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace Amos.AbpLearn.OrderManagement
{
    [DependsOn(
        typeof(OrderManagementHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class OrderManagementConsoleApiClientModule : AbpModule
    {
        
    }
}
