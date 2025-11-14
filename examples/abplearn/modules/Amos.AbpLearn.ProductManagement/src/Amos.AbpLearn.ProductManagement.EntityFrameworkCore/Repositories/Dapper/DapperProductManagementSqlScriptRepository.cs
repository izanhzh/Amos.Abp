using Amos.Abp.Repositories;
using Amos.Abp.SqlScript;
using Amos.AbpLearn.ProductManagement.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace Amos.AbpLearn.ProductManagement.Repositories.Dapper
{
    public class DapperProductManagementSqlScriptRepository : SqlScriptRepository<ProductManagementDbContext>, IProductManagementSqlScriptRepository, ITransientDependency
    {
        public DapperProductManagementSqlScriptRepository(IDbContextProvider<ProductManagementDbContext> dbContextProvider, ISqlScriptProvider sqlScriptProvider) : base(dbContextProvider, sqlScriptProvider)
        {
        }
    }
}
