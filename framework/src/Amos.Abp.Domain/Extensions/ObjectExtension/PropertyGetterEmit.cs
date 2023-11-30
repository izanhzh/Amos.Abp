using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Amos.Abp.Extensions.ObjectExtension
{
    /// <summary>
    /// Emit 动态构造 Get方法
    /// </summary>
    public class PropertyGetterEmit<TObj>
    {
        private readonly Func<TObj, object> getter;

        public PropertyGetterEmit(PropertyInfo propertyInfo)
        {
            //Objcet value = Obj.GetValue(object instance);
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }
            this.getter = CreateGetterEmit(propertyInfo);

        }

        public object Invoke(TObj instance)
        {
            return getter?.Invoke(instance);
        }

        private Func<TObj, object> CreateGetterEmit(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            MethodInfo getMethod = property.GetGetMethod(true);

            DynamicMethod dm = new DynamicMethod("PropertyGetter", typeof(object), new Type[] { typeof(object) }, property.DeclaringType, true);

            ILGenerator il = dm.GetILGenerator();

            if (!getMethod.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
                il.EmitCall(OpCodes.Callvirt, getMethod, null);
            }
            else
                il.EmitCall(OpCodes.Call, getMethod, null);

            if (property.PropertyType.IsValueType)
            {
                il.Emit(OpCodes.Box, property.PropertyType);
            }
            il.Emit(OpCodes.Ret);

            return (Func<TObj, object>)dm.CreateDelegate(typeof(Func<TObj, object>));
        }
    }
}
