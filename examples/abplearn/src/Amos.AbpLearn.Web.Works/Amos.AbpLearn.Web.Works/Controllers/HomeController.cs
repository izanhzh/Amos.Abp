using Microsoft.AspNetCore.Mvc;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amos.AbpLearn.Web.Works.Controllers
{
    public class HomeController : Controller
    {
        private readonly IScheduler _scheduler;

        public HomeController(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public IActionResult Index()
        {
            return Redirect("/Quartz");
        }

        public async Task<IActionResult> CancelAllExecutingJobAsync()
        {
            var executingJobs = await _scheduler.GetCurrentlyExecutingJobs();
            foreach (var executingJob in executingJobs)
            {
                await _scheduler.Interrupt(executingJob.JobDetail.Key);
            }

            return Ok("已停止全部正在执行的Job");
        }
    }
}
