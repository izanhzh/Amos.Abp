using System;

namespace Amos.Abp.TempTable.EntityFrameworkCore
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = true)]
    public class AutoAddTempTableToModelAttribute : Attribute
    {
        public Type FromModule { get; }

        public AutoAddTempTableToModelAttribute(Type fromModule)
        {
            FromModule = fromModule;
        }
    }
}
