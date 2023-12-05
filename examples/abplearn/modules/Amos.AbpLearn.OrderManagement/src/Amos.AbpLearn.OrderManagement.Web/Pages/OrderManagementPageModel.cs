using Amos.AbpLearn.OrderManagement.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Amos.AbpLearn.OrderManagement.Web.Pages
{
    /* Inherit your PageModel classes from this class.
     */
    public abstract class OrderManagementPageModel : AbpPageModel
    {
        protected OrderManagementPageModel()
        {
            LocalizationResourceType = typeof(OrderManagementResource);
            ObjectMapperContext = typeof(OrderManagementWebModule);
        }
    }
}