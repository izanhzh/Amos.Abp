using System;
using System.Collections.Generic;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;

namespace Amos.Abp.EntityFrameworkCore.DependencyInjection
{
    public class AutoAddEntityEfCoreRepositoryRegistrar : EfCoreRepositoryRegistrar
    {
        public AutoAddEntityEfCoreRepositoryRegistrar(AbpDbContextRegistrationOptions options) : base(options)
        {
        }

        protected override IEnumerable<Type> GetEntityTypes(Type dbContextType)
        {
            var entityTypes = EntityFinder.GetAutoAddEntityTypes(dbContextType);
            return entityTypes;
        }
    }
}
