using System;

namespace Amos.Abp.EntityFrameworkCore
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = true)]
    public class AutoAddModelFromModuleAssemblyAttribute : Attribute
    {
        public Type ModuleType { get; }

        public AutoAddModelFromModuleAssemblyAttribute(Type moduleType)
        {
            ModuleType = moduleType;
        }
    }
}
