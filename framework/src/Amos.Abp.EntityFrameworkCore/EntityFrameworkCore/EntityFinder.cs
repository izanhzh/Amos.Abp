using Amos.Abp.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Volo.Abp.Domain.Entities;

namespace Amos.Abp.EntityFrameworkCore
{
    public static class EntityFinder
    {
        private static ConcurrentDictionary<Type, IEnumerable<Type>> _autoAddEntityTypeCache = new ConcurrentDictionary<Type, IEnumerable<Type>>();

        public static IEnumerable<Type> GetEntityTypeFromAssembly(Assembly entityFromAssembly)
        {
            return entityFromAssembly.GetTypes().Where(w => w.IsClass && !w.IsAbstract && w.InheritsOrImplements(typeof(IEntity<>)));
        }

        public static IEnumerable<Type> GetAutoAddEntityTypes(Type dbContextType)
        {
            var isCached = _autoAddEntityTypeCache.TryGetValue(dbContextType, out IEnumerable<Type> result);
            if (isCached)
            {
                return result;
            }

            var dbContextInterfaces = DbContextHelper.GetDbContextImplementsInterfaces(dbContextType);
            var autoAddEntityTypes = GetAutoAddEntityTypesFromModuleAssembly(dbContextInterfaces);
            _autoAddEntityTypeCache.AddOrUpdate(dbContextType, (t_key) => autoAddEntityTypes, (t_key, t_value) => autoAddEntityTypes);
            return autoAddEntityTypes;
        }

        private static IEnumerable<Type> GetAutoAddEntityTypesFromModuleAssembly(IEnumerable<Type> dbContextInterfaces)
        {
            foreach (var dbContextInterface in dbContextInterfaces)
            {
                var entityTypes = GetAutoAddEntityTypesFromModuleAssembly(dbContextInterface);
                foreach (var entityType in entityTypes)
                {
                    yield return entityType;
                }
            }
        }

        private static IEnumerable<Type> GetAutoAddEntityTypesFromModuleAssembly(Type dbContextInterface)
        {
            var attributes = dbContextInterface.GetCustomAttributes<AutoAddModelFromModuleAssemblyAttribute>(true);
            foreach (var attribute in attributes)
            {
                var entityTypes = GetEntityTypeFromAssembly(attribute.ModuleType.Assembly);
                foreach (var entityType in entityTypes)
                {
                    yield return entityType;
                }
            }
        }
    }
}
