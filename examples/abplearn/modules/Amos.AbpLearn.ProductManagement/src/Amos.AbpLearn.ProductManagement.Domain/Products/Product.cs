using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace Amos.AbpLearn.ProductManagement.Products
{
    public class Product : FullAuditedAggregateRoot<long>
    {
        [NotNull]
        public virtual string Name { get; set; }

        [CanBeNull]
        public virtual string Description { get; set; }
    }
}
