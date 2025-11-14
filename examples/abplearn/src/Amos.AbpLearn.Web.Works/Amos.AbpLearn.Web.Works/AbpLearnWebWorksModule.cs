using Amos.AbpLearn.EntityFrameworkCore;
using Amos.AbpLearn.Localization;
using Amos.AbpLearn.MultiTenancy;
using Amos.AbpLearn.ProductManagement;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using SilkierQuartz;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.BackgroundWorkers.Quartz;
using Volo.Abp.IdentityServer.Tokens;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Quartz;

namespace Amos.AbpLearn.Web.Works
{
    [DependsOn(typeof(AbpAutofacModule))]
    [DependsOn(typeof(AbpAspNetCoreMultiTenancyModule))]
    [DependsOn(typeof(AbpAspNetCoreSerilogModule))]
    [DependsOn(typeof(AbpBackgroundWorkersQuartzModule))]
    [DependsOn(typeof(AbpLearnHttpApiModule))]
    [DependsOn(typeof(AbpLearnApplicationModule))]
    [DependsOn(typeof(AbpLearnEntityFrameworkCoreModule))]
    [DependsOn(typeof(ProductManagementDomainModule))]
    public class AbpLearnWebWorksModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(
                    typeof(AbpLearnResource),
                    typeof(AbpLearnDomainModule).Assembly,
                    typeof(AbpLearnDomainSharedModule).Assembly,
                    typeof(AbpLearnApplicationModule).Assembly,
                    typeof(AbpLearnApplicationContractsModule).Assembly,
                    typeof(AbpLearnWebWorksModule).Assembly
                );
            });

            PreConfigureQuartz(configuration);
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            context.Services.AddControllersWithViews().AddNewtonsoftJson();

            ConfigureWorkers();

            ConfigureLocalizationServices();
        }

        private void PreConfigureQuartz(IConfiguration configuration)
        {
            PreConfigure<AbpQuartzOptions>(options =>
            {
                options.Properties = new NameValueCollection
                {
                    //scheduler名字
                    ["quartz.scheduler.instanceName"] = "AbpLearnWebWorks",
                    //序列化类型
                    ["quartz.serializer.type"] = "json",//binary/json
                    //自动生成scheduler实例ID，主要为了保证集群中的实例具有唯一标识
                    ["quartz.scheduler.instanceId"] = "AUTO",
                    //是否配置集群
                    ["quartz.jobStore.clustered"] = "true",
                    //线程池个数
                    ["quartz.threadPool.threadCount"] = "100",
                    ["quartz.threadPool.maxConcurrency"] = "100",
                    //类型为JobStoreXT,事务
                    ["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz",
                    //以下配置需要数据库表配合使用，表结构sql地址：https://github.com/quartznet/quartznet/tree/master/database/tables
                    //JobDataMap中的数据都是字符串
                    ["quartz.jobStore.useProperties"] = "true",
                    //数据源名称
                    ["quartz.jobStore.dataSource"] = "myDS",
                    //数据表名前缀
                    ["quartz.jobStore.tablePrefix"] = "QRTZ_",
                    //使用Sqlserver的Ado操作代理类
                    ["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz",
                    //数据源连接字符串
                    ["quartz.dataSource.myDS.connectionString"] = configuration.GetConnectionString("Default"),
                    //数据源的数据库
                    ["quartz.dataSource.myDS.provider"] = "SqlServer",
                    //执行历史记录
                    ["quartz.plugin.recentHistory.type"] = "Quartz.Plugins.RecentHistory.ExecutionHistoryPlugin, Quartz.Plugins.RecentHistory",
                    ["quartz.plugin.recentHistory.storeType"] = "Quartz.Plugins.RecentHistory.Impl.InProcExecutionHistoryStore, Quartz.Plugins.RecentHistory",
                };
            });
        }

        private void ConfigureWorkers()
        {
            Configure<AbpBackgroundJobOptions>(options =>
            {
                options.IsJobExecutionEnabled = false; //Disables job execution
            });
            Configure<TokenCleanupOptions>(options =>
            {
                options.IsCleanupEnabled = false;
            });
            Configure<AbpBackgroundWorkerQuartzOptions>(options =>
            {
                options.IsAutoRegisterEnabled = false;
            });
        }

        private void ConfigureLocalizationServices()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("ar", "ar", "العربية"));
                options.Languages.Add(new LanguageInfo("cs", "cs", "Čeština"));
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("en-GB", "en-GB", "English (UK)"));
                options.Languages.Add(new LanguageInfo("hu", "hu", "Magyar"));
                options.Languages.Add(new LanguageInfo("fi", "fi", "Finnish"));
                options.Languages.Add(new LanguageInfo("fr", "fr", "Français"));
                options.Languages.Add(new LanguageInfo("hi", "hi", "Hindi"));
                options.Languages.Add(new LanguageInfo("it", "it", "Italian"));
                options.Languages.Add(new LanguageInfo("pt-BR", "pt-BR", "Português"));
                options.Languages.Add(new LanguageInfo("ru", "ru", "Русский"));
                options.Languages.Add(new LanguageInfo("sk", "sk", "Slovak"));
                options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
                options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "繁體中文"));
                options.Languages.Add(new LanguageInfo("de-DE", "de-DE", "Deutsch"));
                options.Languages.Add(new LanguageInfo("es", "es", "Español"));
            });
        }

        public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAbpRequestLocalization();


            app.UseStaticFiles();
            app.UseRouting();

            if (MultiTenancyConsts.IsEnabled)
            {
                app.UseMultiTenancy();
            }

            //app.UseAuditing();
            app.UseAbpSerilogEnrichers();

            var scheduler = context.ServiceProvider.GetRequiredService<IScheduler>();
            app.UseSilkierQuartz(new SilkierQuartzOptions()
            {
                Scheduler = scheduler,
                ProductName = "AbpLearnWebWorks",
                VirtualPathRoot = "/Quartz",
                UseLocalTime = true,
                DefaultDateFormat = "yyyy-MM-dd",
                DefaultTimeFormat = "HH:mm:ss",
            });
            //注意：AbpBackgroundWorkerQuartzOptions IsAutoRegisterEnabled设置为false，交由这里进行自定义注册
            var works = context.ServiceProvider.GetServices<IQuartzBackgroundWorker>().Where(x => x.AutoRegister);
            var backgroundWorkerManager = context.ServiceProvider.GetRequiredService<IBackgroundWorkerManager>();
            foreach (var work in works)
            {
                if (work.JobDetail != null && !(await scheduler.CheckExists(work.JobDetail.Key)))
                {
                    if (work.Trigger == null)
                    {
                        await scheduler.AddJob(work.JobDetail, true, true);
                    }
                    else
                    {
                        await backgroundWorkerManager.AddAsync(work);
                    }
                }
            }

            app.UseConfiguredEndpoints();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute("default", "{controller=home}/{action=Index}/{id?}");
            });
        }
    }
}
