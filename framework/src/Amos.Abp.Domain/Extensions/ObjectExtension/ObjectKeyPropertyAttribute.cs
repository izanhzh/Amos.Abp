using System;

namespace Amos.Abp.Extensions.ObjectExtension
{
    /// <summary>
    /// 对象Key属性特性标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ObjectKeyPropertyAttribute : Attribute
    {
    }
}
