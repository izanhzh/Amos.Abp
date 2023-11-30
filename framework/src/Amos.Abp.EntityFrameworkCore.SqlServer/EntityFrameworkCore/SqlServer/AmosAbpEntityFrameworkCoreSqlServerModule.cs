using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Modularity;

namespace Amos.Abp.EntityFrameworkCore.SqlServer
{
    [DependsOn(typeof(AmosAbpEntityFrameworkCoreModule))]
    public class AmosAbpEntityFrameworkCoreSqlServerModule : AbpModule
    {
    }
}
