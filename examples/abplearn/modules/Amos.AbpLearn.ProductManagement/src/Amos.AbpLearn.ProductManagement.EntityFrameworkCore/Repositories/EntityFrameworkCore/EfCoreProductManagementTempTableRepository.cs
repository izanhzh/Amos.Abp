using Amos.Abp.Domain.Repositories;
using Amos.AbpLearn.ProductManagement.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace Amos.AbpLearn.ProductManagement.Repositories.EntityFrameworkCore
{
    public class EfCoreProductManagementTempTableRepository : EfCoreTempTableRepository<IProductManagementDbContext>, IProductManagementTempTableRepository, ITransientDependency
    {
        public EfCoreProductManagementTempTableRepository(IDbContextProvider<IProductManagementDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
