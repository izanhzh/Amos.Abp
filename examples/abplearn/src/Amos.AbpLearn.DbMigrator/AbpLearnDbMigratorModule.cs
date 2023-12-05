using Amos.AbpLearn.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace Amos.AbpLearn.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpLearnEntityFrameworkCoreModule),
        typeof(AbpLearnApplicationContractsModule)
        )]
    public class AbpLearnDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}
