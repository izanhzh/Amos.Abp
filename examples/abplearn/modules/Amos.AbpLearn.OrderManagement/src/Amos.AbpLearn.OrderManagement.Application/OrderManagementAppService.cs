using Amos.AbpLearn.OrderManagement.Localization;
using Volo.Abp.Application.Services;

namespace Amos.AbpLearn.OrderManagement
{
    public abstract class OrderManagementAppService : ApplicationService
    {
        protected OrderManagementAppService()
        {
            LocalizationResource = typeof(OrderManagementResource);
            ObjectMapperContext = typeof(OrderManagementApplicationModule);
        }
    }
}
