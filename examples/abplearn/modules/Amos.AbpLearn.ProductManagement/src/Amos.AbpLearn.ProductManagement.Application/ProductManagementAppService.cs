using Amos.AbpLearn.ProductManagement.Localization;
using Volo.Abp.Application.Services;

namespace Amos.AbpLearn.ProductManagement
{
    public abstract class ProductManagementAppService : ApplicationService
    {
        protected ProductManagementAppService()
        {
            LocalizationResource = typeof(ProductManagementResource);
            ObjectMapperContext = typeof(ProductManagementApplicationModule);
        }
    }
}
