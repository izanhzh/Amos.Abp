using Amos.AbpLearn.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Amos.AbpLearn.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class AbpLearnController : AbpController
    {
        protected AbpLearnController()
        {
            LocalizationResource = typeof(AbpLearnResource);
        }
    }
}