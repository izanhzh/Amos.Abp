using Amos.Abp.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;

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

            var dbContextInterfaces = GetDbContextImplementsInterfaces(dbContextType);
            var autoAddEntityTypes = GetAutoAddEntityTypesFromModuleAssembly(dbContextInterfaces);
            _autoAddEntityTypeCache.AddOrUpdate(dbContextType, (t_key) => autoAddEntityTypes, (t_key, t_value) => autoAddEntityTypes);
            return autoAddEntityTypes;
        }

        private static IEnumerable<Type> GetDbContextImplementsInterfaces(Type dbContextType)
        {
            if (dbContextType.IsInterface && dbContextType.InheritsOrImplements(typeof(IEfCoreDbContext)))
            {
                yield return dbContextType;
            }
            var interfaces = dbContextType.GetInterfaces().Where(w => w.InheritsOrImplements(typeof(IEfCoreDbContext)));
            foreach (var entry in interfaces)
            {
                var entryInterfaces = GetDbContextImplementsInterfaces(entry);
                foreach (var entryInterface in entryInterfaces)
                {
                    yield return entryInterface;
                }
            }
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
            var attributes = dbContextInterface.GetCustomAttributes<AutoAddEntityToModelAttribute>(true);
            foreach (var attribute in attributes)
            {
                var entityTypes = GetEntityTypeFromAssembly(attribute.FromModule.Assembly);
                foreach (var entityType in entityTypes)
                {
                    yield return entityType;
                }
            }
        }
    }
}
