using Amos.Abp.EntityFrameworkCore;
using Amos.Abp.TempTable;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;

namespace Amos.Abp.Domain.Repositories
{
    public abstract class EfCoreTempTableRepository<TDbContext> : ITempTableRepository where TDbContext : IEfCoreDbContext
    {
        private readonly IDbContextProvider<TDbContext> _dbContextProvider;

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
    }
}
