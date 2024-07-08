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
    public abstract class EfCoreTempTableRepository<TDbContext> : ITempTableRepository where TDbContext : DbContext, IEfCoreDbContext
    {
        private readonly IDbContextProvider<TDbContext> _dbContextProvider;

        public bool? IsChangeTrackingEnabled { get; }

        public EfCoreTempTableRepository(IDbContextProvider<TDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public virtual async Task<IQueryable<TTempTable>> InsertIntoTempTableAsync<TTempTable>(IEnumerable<TTempTable> entities) where TTempTable : class, ITempTable
        {
            var dbContext = await _dbContextProvider.GetDbContextAsync();
            return await dbContext.InsertIntoTempTableAsync(entities, new TempTableInsertOptions
            {
                PrimaryKeyCreation = TempTablePrimaryKeyCreation.None,
                TempTableCreationOptions = new TempTableCreationOptions() { MakeTableNameUnique = true }
            });
        }

        public virtual async Task<string> InsertIntoTempTableAndGetTableNameAsync<TTempTable>(IEnumerable<TTempTable> entities) where TTempTable : class, ITempTable
        {
            var dbContext = await _dbContextProvider.GetDbContextAsync();
            return await dbContext.InsertIntoTempTableAndGetTableNameAsync(entities, new TempTableInsertOptions
            {
                PrimaryKeyCreation = TempTablePrimaryKeyCreation.None,
                TempTableCreationOptions = new TempTableCreationOptions() { MakeTableNameUnique = true }
            });
        }
    }
}
