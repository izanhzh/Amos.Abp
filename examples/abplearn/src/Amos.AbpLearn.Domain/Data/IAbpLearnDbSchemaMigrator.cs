using System.Threading.Tasks;

namespace Amos.AbpLearn.Data
{
    public interface IAbpLearnDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
