using Amos.Abp.EntityFrameworkCore;
using Amos.Abp.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

            var dbContextInterfaces = DbContextHelper.GetDbContextImplementsInterfaces(dbContextType);
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

        private static IEnumerable<Type> GetAutoAddTempTablesFromModuleAssembly(Type dbContextInterface)
        {
            var attributes = dbContextInterface.GetCustomAttributes<AutoAddModelFromModuleAssemblyAttribute>(true);
            foreach (var attribute in attributes)
            {
                var tempTables = GetTempTableFromAssembly(attribute.ModuleType.Assembly);
                foreach (var tempTable in tempTables)
                {
                    yield return tempTable;
                }
            }
        }
    }
}
