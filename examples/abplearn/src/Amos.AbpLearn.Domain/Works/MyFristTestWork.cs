using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers.Quartz;

namespace Amos.AbpLearn.Works
{
    public class MyFristTestWork : QuartzBackgroundWorkerBase
    {
        public MyFristTestWork()
        {
            JobDetail = JobBuilder.Create<MyFristTestWork>().WithIdentity(nameof(MyFristTestWork), "System").WithDescription("我的第一个测试Work").Build();
            Trigger = TriggerBuilder.Create().WithIdentity(nameof(MyFristTestWork), "System").WithCronSchedule("0 0/1 * * * ?").Build();
        }

        public override Task Execute(IJobExecutionContext context)
        {
            Logger.LogInformation("Executed MyFristTestWork..!"); 
            return Task.CompletedTask;
        }
    }
}
