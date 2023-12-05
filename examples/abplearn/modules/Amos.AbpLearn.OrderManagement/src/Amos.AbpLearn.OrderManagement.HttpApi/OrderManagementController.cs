using Amos.AbpLearn.OrderManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Amos.AbpLearn.OrderManagement
{
    public abstract class OrderManagementController : AbpController
    {
        protected OrderManagementController()
        {
            LocalizationResource = typeof(OrderManagementResource);
        }
    }
}
