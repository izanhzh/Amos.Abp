using System;
using System.Linq;

namespace Amos.Abp.Extensions.ObjectExtension
{
    /// <summary>
    /// 对象Key属性值相等比较
    /// </summary>
    public class ObjectKeyPropertyEqualityComparer<T> : ObjectPropertyEqualityComparer<T>
        where T : class
    {
        public static string[] KeyProperties { get; }

        static ObjectKeyPropertyEqualityComparer()
        {
            KeyProperties = PropertyEmit<T>.GetPropertyEmits().Where(w => w.Value.Info.IsDefined(typeof(ObjectKeyPropertyAttribute), true)).Select(s => s.Key).ToArray();
            if (KeyProperties == null || KeyProperties.Length == 0)
            {
                throw new Exception($"类型“{typeof(T).FullName}”缺少用ObjectKeyPropertyAttribute特性标记的属性");
            }
        }

        public ObjectKeyPropertyEqualityComparer() :
            base(KeyProperties)
        {
        }

        public ObjectKeyPropertyEqualityComparer(Action<ObjectPropertyEqualityComparerOptions> configureOptions)
            : base(configureOptions, KeyProperties)
        {
        }
    }
}
