using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers.Quartz;

namespace Amos.AbpLearn.ProductManagement.Works
{
    [DisallowConcurrentExecution]
    public class ProductManagementTestWork : QuartzBackgroundWorkerBase
    {
        public ProductManagementTestWork()
        {
            JobDetail = JobBuilder.Create<ProductManagementTestWork>().WithIdentity(nameof(ProductManagementTestWork), "ProductManagement").WithDescription("子模块测试Work").Build();
            //Trigger = TriggerBuilder.Create().WithIdentity(nameof(ProductManagementTestWork), "ProductManagement").WithSimpleSchedule(a => a.WithIntervalInSeconds(10).RepeatForever()).Build();
            //AutoRegister = false;
        }

        public override async Task Execute(IJobExecutionContext context)
        {
            //while (!context.CancellationToken.IsCancellationRequested)
            //{
            //    Logger.LogInformation("Executed ProductManagementTestWork..!");

            //    await Task.Delay(2000);
            //}
            Logger.LogInformation("****************************************************************开始!");
            await Task.Delay(10000);
            Logger.LogInformation("****************************************************************结束!");
        }
    }
}
