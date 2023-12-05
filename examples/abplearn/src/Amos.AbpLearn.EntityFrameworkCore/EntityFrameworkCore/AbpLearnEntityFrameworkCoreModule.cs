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
            //Ŀǰabp vnext 4.4.4�汾�������ص㣬��ȷ���Ժ�汾�᲻�����
            //���滻��DbContext�����������ģ����ע���Ĭ��Repository (AddDefaultRepositories)�����޷���ʹ������ģ������滻 (ReplaceDbContext)�����ܸ��ǣ������Զ���Repositoryȴ���滻
            //��˰�����Լ������
            //1. ����ģ���У���Ҫע��Ĭ��Repository (AddDefaultRepositories)������ע���Զ���Repository
            //2. ����ģ���У��������ģ���DbContext�����滻
            //3. ��������滻��ģ�飬��ģ�����ʹ���Լ���DbContext�Լ�Ĭ��Repository����������ģ���DbContext��������AddAbpDbContext��ע��Ĭ��Repository
            //   context.Services.AddAbpDbContext<���滻����ģ��DbContext>(options =>
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
