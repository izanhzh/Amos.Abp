using Amos.Abp.EntityFrameworkCore;
using Amos.Abp.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Volo.Abp.EntityFrameworkCore;

namespace Amos.Abp.TempTable.EntityFrameworkCore
{
    public static class TempTableFinder
    {
        private static ConcurrentDictionary<Type, IEnumerable<Type>> _autoAddTempTableCache = new ConcurrentDictionary<Type, IEnumerable<Type>>();

        public static IEnumerable<Type> GetTempTableFromAssembly(Assembly tempTableFromAssembly)
        {
            return tempTableFromAssembly.GetTypes().Where(w => w.IsClass && !w.IsAbstract && w.InheritsOrImplements(typeof(ITempTable)));
        }

        public static IEnumerable<Type> GetAutoAddTempTables(Type dbContextType)
        {
            var isCached = _autoAddTempTableCache.TryGetValue(dbContextType, out IEnumerable<Type> result);
            if (isCached)
            {
                return result;
            }

            var dbContextInterfaces = GetDbContextImplementsInterfaces(dbContextType);
            var autoAddTempTables = GetAutoAddTempTablesFromModuleAssembly(dbContextInterfaces);
            _autoAddTempTableCache.AddOrUpdate(dbContextType, (t_key) => autoAddTempTables, (t_key, t_value) => autoAddTempTables);
            return autoAddTempTables;
        }

        private static IEnumerable<Type> GetAutoAddTempTablesFromModuleAssembly(IEnumerable<Type> dbContextInterfaces)
        {
            foreach (var dbContextInterface in dbContextInterfaces)
            {
                var tempTables = GetAutoAddTempTablesFromModuleAssembly(dbContextInterface);
                foreach (var tempTable in tempTables)
                {
                    yield return tempTable;
                }
            }
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

        private static IEnumerable<Type> GetAutoAddTempTablesFromModuleAssembly(Type dbContextInterface)
        {
            var attributes = dbContextInterface.GetCustomAttributes<AutoAddTempTableToModelAttribute>(true);
            foreach (var attribute in attributes)
            {
                var tempTables = GetTempTableFromAssembly(attribute.FromModule.Assembly);
                foreach (var tempTable in tempTables)
                {
                    yield return tempTable;
                }
            }
        }
    }
}
