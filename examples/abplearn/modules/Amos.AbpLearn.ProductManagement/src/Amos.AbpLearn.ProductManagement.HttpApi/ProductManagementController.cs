using Amos.AbpLearn.ProductManagement.Localization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Amos.AbpLearn.ProductManagement
{
    [Route("product-management")]
    public abstract class ProductManagementController : AbpController
    {
        protected ProductManagementController()
        {
            LocalizationResource = typeof(ProductManagementResource);
        }
    }
}
