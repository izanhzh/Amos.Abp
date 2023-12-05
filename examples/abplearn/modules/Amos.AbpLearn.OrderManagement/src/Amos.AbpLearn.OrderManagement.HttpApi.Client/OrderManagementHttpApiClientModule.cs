using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Amos.AbpLearn.OrderManagement
{
    [DependsOn(
        typeof(OrderManagementApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class OrderManagementHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "OrderManagement";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(OrderManagementApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
