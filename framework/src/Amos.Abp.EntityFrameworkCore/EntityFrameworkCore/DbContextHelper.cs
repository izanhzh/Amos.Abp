using Amos.Abp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.EntityFrameworkCore;

namespace Amos.Abp.EntityFrameworkCore
{
    public static class DbContextHelper
    {
        public static IEnumerable<Type> GetDbContextImplementsInterfaces(Type dbContextType)
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
    }
}
