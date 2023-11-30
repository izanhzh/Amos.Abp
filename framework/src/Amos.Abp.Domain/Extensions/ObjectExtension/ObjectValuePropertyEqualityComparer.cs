using System;
using System.Linq;

namespace Amos.Abp.Extensions.ObjectExtension
{
    /// <summary>
    /// 对象value属性值相等比较
    /// </summary>
    public class ObjectValuePropertyEqualityComparer<T> : ObjectPropertyEqualityComparer<T>
        where T : class
    {
        public static string[] ValueProperties { get; }

        static ObjectValuePropertyEqualityComparer()
        {
            ValueProperties = PropertyEmit<T>.GetPropertyEmits()
                .Where(w => !w.Value.Info.IsDefined(typeof(ObjectKeyPropertyAttribute), true) && !w.Value.Info.IsDefined(typeof(ObjectValuePropertyIgnoreAttribute), true))
                .Select(s => s.Key)
                .ToArray();
            if (ValueProperties == null || ValueProperties.Length == 0)
            {
                throw new Exception($"类型“{typeof(T).FullName}”缺少可作为Value的属性");
            }
        }

        public ObjectValuePropertyEqualityComparer() :
            base(ValueProperties)
        {
        }

        public ObjectValuePropertyEqualityComparer(Action<ObjectPropertyEqualityComparerOptions> configureOptions)
            : base(configureOptions, ValueProperties)
        {
        }
    }
}
