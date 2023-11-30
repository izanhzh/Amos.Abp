using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Amos.Abp.Extensions.ObjectExtension
{
    /// <summary>
    /// Emit动态构造Set方法
    /// </summary>
    public class PropertySetterEmit<TObj>
    {
        private readonly Action<TObj, object> setFunc;

        public PropertySetterEmit(PropertyInfo propertyInfo)
        {
            //Obj.Set(Object instance,Object value)
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }
            this.setFunc = CreatePropertySetter(propertyInfo);

        }

        private Action<TObj, object> CreatePropertySetter(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            MethodInfo setMethod = property.GetSetMethod(true);

            DynamicMethod dm = new DynamicMethod("PropertySetter", null, new Type[] { typeof(object), typeof(object) }, property.DeclaringType, true);

            ILGenerator il = dm.GetILGenerator();

            if (!setMethod.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
            }
            il.Emit(OpCodes.Ldarg_1);

            EmitCastToReference(il, property.PropertyType);
            if (!setMethod.IsStatic && !property.DeclaringType.IsValueType)
            {
                il.EmitCall(OpCodes.Callvirt, setMethod, null);
            }
            else
            {
                il.EmitCall(OpCodes.Call, setMethod, null);
            }

            il.Emit(OpCodes.Ret);
            return (Action<TObj, object>)dm.CreateDelegate(typeof(Action<TObj, object>));
        }

        private static void EmitCastToReference(ILGenerator il, Type type)
        {
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, type);
            }
            else
            {
                il.Emit(OpCodes.Castclass, type);
            }
        }

        public void Invoke(TObj instance, object value)
        {
            this.setFunc?.Invoke(instance, value);
        }
    }
}
