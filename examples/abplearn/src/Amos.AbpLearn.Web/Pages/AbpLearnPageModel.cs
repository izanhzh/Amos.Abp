using Amos.AbpLearn.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Amos.AbpLearn.Web.Pages
{
    /* Inherit your PageModel classes from this class.
     */
    public abstract class AbpLearnPageModel : AbpPageModel
    {
        protected AbpLearnPageModel()
        {
            LocalizationResourceType = typeof(AbpLearnResource);
        }
    }
}