using Amos.AbpLearn.ProductManagement.Products;
using Volo.Abp.Domain.Repositories;

namespace Amos.AbpLearn.ProductManagement.Repositories
{
    public interface IProductRepository : IRepository<Product, long>
    {
    }
}
