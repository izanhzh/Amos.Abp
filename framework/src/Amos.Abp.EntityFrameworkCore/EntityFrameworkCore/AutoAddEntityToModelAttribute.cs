using System;

namespace Amos.Abp.EntityFrameworkCore
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = true)]
    public class AutoAddEntityToModelAttribute : Attribute
    {
        public Type FromModule { get; }

        public AutoAddEntityToModelAttribute(Type fromModule)
        {
            FromModule = fromModule;
        }
    }
}
