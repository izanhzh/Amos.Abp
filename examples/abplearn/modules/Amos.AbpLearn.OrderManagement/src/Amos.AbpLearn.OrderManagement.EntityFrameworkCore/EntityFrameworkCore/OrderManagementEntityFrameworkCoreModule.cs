using Amos.Abp.SqlScript;
using Amos.AbpLearn.OrderManagement.SqlScript;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Amos.AbpLearn.OrderManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(OrderManagementDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    [DependsOn(typeof(AmosAbpSqlScriptModule))]
    public class OrderManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<OrderManagementDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
            });

            Configure<SqlScriptOptions>(options =>
            {
                options.AddResource("OrderManagement", typeof(OrderManagementSqlScriptResource));
            });
        }
    }
}