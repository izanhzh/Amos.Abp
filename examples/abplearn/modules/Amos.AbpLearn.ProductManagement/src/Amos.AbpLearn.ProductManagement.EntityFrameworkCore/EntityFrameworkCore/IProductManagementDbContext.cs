using Amos.Abp.EntityFrameworkCore;
using Amos.Abp.TempTable.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Amos.AbpLearn.ProductManagement.EntityFrameworkCore
{
    [AutoAddEntityToModel(typeof(ProductManagementDomainModule))]
    [AutoAddTempTableToModel(typeof(ProductManagementDomainModule))]
    [ConnectionStringName(ProductManagementDbProperties.ConnectionStringName)]
    public interface IProductManagementDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */
        //DbSet<Product> Product { get; }
    }
}