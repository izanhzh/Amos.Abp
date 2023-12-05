using System;
using System.Collections.Generic;
using System.Text;
using Amos.AbpLearn.Localization;
using Volo.Abp.Application.Services;

namespace Amos.AbpLearn
{
    /* Inherit your application services from this class.
     */
    public abstract class AbpLearnAppService : ApplicationService
    {
        protected AbpLearnAppService()
        {
            LocalizationResource = typeof(AbpLearnResource);
        }
    }
}
