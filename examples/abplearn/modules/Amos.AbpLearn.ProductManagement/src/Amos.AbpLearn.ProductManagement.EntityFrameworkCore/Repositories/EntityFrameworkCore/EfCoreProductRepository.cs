using Amos.AbpLearn.ProductManagement.EntityFrameworkCore;
using Amos.AbpLearn.ProductManagement.Products;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Amos.AbpLearn.ProductManagement.Repositories.EntityFrameworkCore
{
    public class EfCoreProductRepository : EfCoreRepository<IProductManagementDbContext, Product, long>, IProductRepository
    {
        private readonly IDbContextProvider<IProductManagementDbContext> _dbContextProvider;

        public EfCoreProductRepository(IDbContextProvider<IProductManagementDbContext> dbContextProvider) : base(dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public override async Task<IQueryable<Product>> GetQueryableAsync()
        {
            var a = await _dbContextProvider.GetDbContextAsync();
            return await base.GetQueryableAsync();
        }
    }
}
