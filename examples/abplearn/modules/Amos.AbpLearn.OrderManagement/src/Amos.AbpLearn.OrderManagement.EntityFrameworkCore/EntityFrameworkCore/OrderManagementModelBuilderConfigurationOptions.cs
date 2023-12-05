using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Amos.AbpLearn.OrderManagement.EntityFrameworkCore
{
    public class OrderManagementModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public OrderManagementModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}