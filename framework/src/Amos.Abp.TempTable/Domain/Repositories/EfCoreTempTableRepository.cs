using Amos.Abp.EntityFrameworkCore;
using Amos.Abp.TempTable;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore;

namespace Amos.Abp.Domain.Repositories
{
    public abstract class EfCoreTempTableRepository<TDbContext> : EfCoreTempTableRepository<TDbContext, TDbContext>
        where TDbContext : DbContext, IEfCoreDbContext
    {
        protected EfCoreTempTableRepository(IDbContextProvider<TDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }

    public abstract class EfCoreTempTableRepository<TDbContext, IDbContext> : ITempTableRepository
        where TDbContext : DbContext, IDbContext
        where IDbContext : IEfCoreDbContext
    {
        private readonly IDbContextProvider<IDbContext> _dbContextProvider;

        public EfCoreTempTableRepository(IDbContextProvider<IDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public virtual async Task<IQueryable<TTempTable>> InsertIntoTempTableAsync<TTempTable>(IEnumerable<TTempTable> entities) where TTempTable : class, ITempTable
        {
            var dbContext = await _dbContextProvider.GetDbContextAsync();
            return await ((TDbContext)dbContext).InsertIntoTempTableAsync(entities, new TempTableInsertOptions
            {
                PrimaryKeyCreation = TempTablePrimaryKeyCreation.None,
                TempTableCreationOptions = new TempTableCreationOptions() { MakeTableNameUnique = true }
            });
        }

        public virtual async Task<string> InsertIntoTempTableAndGetTableNameAsync<TTempTable>(IEnumerable<TTempTable> entities) where TTempTable : class, ITempTable
        {
            var dbContext = await _dbContextProvider.GetDbContextAsync();
            return await ((TDbContext)dbContext).InsertIntoTempTableAndGetTableNameAsync(entities, new TempTableInsertOptions
            {
                PrimaryKeyCreation = TempTablePrimaryKeyCreation.None,
                TempTableCreationOptions = new TempTableCreationOptions() { MakeTableNameUnique = true }
            });
        }
    }
}
