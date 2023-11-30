using Amos.Abp.TempTable;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Amos.Abp.Domain.Repositories
{
    public interface ITempTableRepository : IRepository
    {
        Task<IQueryable<TTempTable>> InsertIntoTempTableAsync<TTempTable>(IEnumerable<TTempTable> entities) where TTempTable : class, ITempTable;
    }
}
