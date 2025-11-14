using Amos.Abp.EntityFrameworkCore;
using Amos.Abp.TempTable;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;

namespace Amos.Abp.Domain.Repositories
{
    public abstract class EfCoreTempTableRepository<TDbContext> : ITempTableRepository
        where TDbContext : IEfCoreDbContext
    {
        private readonly IDbContextProvider<TDbContext> _dbContextProvider;

        public EfCoreTempTableRepository(IDbContextProvider<TDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public virtual async Task<IQueryable<TTempTable>> InsertIntoTempTableAsync<TTempTable>(IEnumerable<TTempTable> entities) where TTempTable : class, ITempTable
        {
            var dbContext = await _dbContextProvider.GetDbContextAsync();
            if (dbContext is DbContext context)
            {
                return await context.InsertIntoTempTableAsync(entities, new TempTableInsertOptions
                {
                    PrimaryKeyCreation = TempTablePrimaryKeyCreation.None,
                    TempTableCreationOptions = new TempTableCreationOptions() { MakeTableNameUnique = true }
                });
            }
            else
            {
                throw new InvalidOperationException("The provided dbContext is not of type DbContext.");
            }
        }

        public virtual async Task<string> InsertIntoTempTableAndGetTableNameAsync<TTempTable>(IEnumerable<TTempTable> entities) where TTempTable : class, ITempTable
        {
            var dbContext = await _dbContextProvider.GetDbContextAsync();
            if (dbContext is DbContext context)
            {
                return await context.InsertIntoTempTableAndGetTableNameAsync(entities, new TempTableInsertOptions
                {
                    PrimaryKeyCreation = TempTablePrimaryKeyCreation.None,
                    TempTableCreationOptions = new TempTableCreationOptions() { MakeTableNameUnique = true }
                });
            }
            else
            {
                throw new InvalidOperationException("The provided dbContext is not of type DbContext.");
            }
        }
    }
}
