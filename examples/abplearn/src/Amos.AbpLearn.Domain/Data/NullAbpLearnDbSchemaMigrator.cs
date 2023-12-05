using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Amos.AbpLearn.Data
{
    /* This is used if database provider does't define
     * IAbpLearnDbSchemaMigrator implementation.
     */
    public class NullAbpLearnDbSchemaMigrator : IAbpLearnDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}