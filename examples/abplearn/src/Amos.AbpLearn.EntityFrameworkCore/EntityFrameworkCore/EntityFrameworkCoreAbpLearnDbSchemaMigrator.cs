using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Amos.AbpLearn.Data;
using Volo.Abp.DependencyInjection;

namespace Amos.AbpLearn.EntityFrameworkCore
{
    public class EntityFrameworkCoreAbpLearnDbSchemaMigrator
        : IAbpLearnDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCoreAbpLearnDbSchemaMigrator(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            /* We intentionally resolving the AbpLearnDbContext
             * from IServiceProvider (instead of directly injecting it)
             * to properly get the connection string of the current tenant in the
             * current scope.
             */

            await _serviceProvider
                .GetRequiredService<AbpLearnDbContext>()
                .Database
                .MigrateAsync();
        }
    }
}
