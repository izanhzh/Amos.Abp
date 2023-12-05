using Amos.Abp.Microsoft.Extensions.DependencyInjection;
using Amos.Abp.TempTable.SqlServer;
using Amos.AbpLearn.OrderManagement.EntityFrameworkCore;
using Amos.AbpLearn.ProductManagement.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace Amos.AbpLearn.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpLearnDomainModule),
        typeof(AbpIdentityEntityFrameworkCoreModule),
        typeof(AbpIdentityServerEntityFrameworkCoreModule),
        typeof(AbpPermissionManagementEntityFrameworkCoreModule),
        typeof(AbpSettingManagementEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreSqlServerModule),
        typeof(AbpBackgroundJobsEntityFrameworkCoreModule),
        typeof(AbpAuditLoggingEntityFrameworkCoreModule),
        typeof(AbpTenantManagementEntityFrameworkCoreModule),
        typeof(AbpFeatureManagementEntityFrameworkCoreModule)
        )]
    [DependsOn(typeof(ProductManagementEntityFrameworkCoreModule))]
    [DependsOn(typeof(OrderManagementEntityFrameworkCoreModule))]
    [DependsOn(typeof(AmosAbpTempTableSqlServerModule))]
    public class AbpLearnEntityFrameworkCoreModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            AbpLearnEfCoreEntityExtensionMappings.Configure();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            //目前abp vnext 4.4.4版本有如下特点，不确定以后版本会不会调整
            //被替换的DbContext，如果其所在模块中注册过默认Repository (AddDefaultRepositories)，则无法被使用它的模块进行替换 (ReplaceDbContext)，不能覆盖，但是自定义Repository却能替换
            //因此按如下约定开发
            //1. 在子模块中，不要注册默认Repository (AddDefaultRepositories)，可以注册自定义Repository
            //2. 在主模块中，必须对子模块的DbContext进行替换
            //3. 如果不想替换子模块，子模块独立使用自己的DbContext以及默认Repository，则必须对子模块的DbContext单独进行AddAbpDbContext并注册默认Repository
            //   context.Services.AddAbpDbContext<不替换的子模块DbContext>(options =>
            //   {
            //      options.AddDefaultRepositories(includeAllEntities: true);
            //   });
            context.Services.AddAbpDbContextEx<AbpLearnDbContext>(options =>
            {
                /* Remove "includeAllEntities: true" to create
                    * default repositories only for aggregate roots */
                options.AddDefaultRepositories(includeAllEntities: true);
            });


            Configure<AbpDbContextOptions>(options =>
            {
                /* The main point to change your DBMS.
                    * See also AbpLearnMigrationsDbContextFactory for EF Core tooling. */
                options.UseSqlServer();
            });
        }
    }
}
