using Amos.Abp.EntityFrameworkCore;
using Amos.Abp.SqlScript;
using Amos.AbpLearn.ProductManagement.SqlScript;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Amos.AbpLearn.ProductManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(ProductManagementDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    [DependsOn(typeof(AmosAbpEntityFrameworkCoreModule))]
    [DependsOn(typeof(AmosAbpSqlScriptModule))]
    public class ProductManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<ProductManagementDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
                //options.AddRepository<Product, EfCoreProductRepository>();
            });

            Configure<SqlScriptOptions>(options =>
            {
                options.AddResource("ProductManagement", typeof(ProductManagementSqlScriptResource));
            });
        }
    }
}