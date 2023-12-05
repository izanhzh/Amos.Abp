using Amos.AbpLearn.ProductManagement.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Amos.AbpLearn.ProductManagement.EntityTypeConfigurations
{
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasIndex(h => h.Name).HasFilter("IsDeleted=0").IsUnique();
    }
}
}
