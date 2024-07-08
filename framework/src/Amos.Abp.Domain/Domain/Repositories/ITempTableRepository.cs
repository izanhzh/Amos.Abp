using Amos.Abp.TempTable;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amos.Abp.Domain.Repositories
{
    public interface ITempTableRepository
    {
        Task<IQueryable<TTempTable>> InsertIntoTempTableAsync<TTempTable>(IEnumerable<TTempTable> entities) where TTempTable : class, ITempTable;

        Task<string> InsertIntoTempTableAndGetTableNameAsync<TTempTable>(IEnumerable<TTempTable> entities) where TTempTable : class, ITempTable;
    }
}
