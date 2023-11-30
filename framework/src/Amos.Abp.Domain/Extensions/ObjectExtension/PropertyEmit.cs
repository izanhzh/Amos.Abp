using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Amos.Abp.Extensions.ObjectExtension
{
    public class PropertyEmit<TObj>
    {
        private static readonly ConcurrentDictionary<Type, Dictionary<string, PropertyEmit<TObj>>> securityCache = new ConcurrentDictionary<Type, Dictionary<string, PropertyEmit<TObj>>>();

        private readonly PropertySetterEmit<TObj> setter;
        private readonly PropertyGetterEmit<TObj> getter;

        public string PropertyName { get; private set; }
        public PropertyInfo Info { get; private set; }

        public PropertyEmit(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            if (propertyInfo.CanWrite)
            {
                setter = new PropertySetterEmit<TObj>(propertyInfo);
            }

            if (propertyInfo.CanRead)
            {
                getter = new PropertyGetterEmit<TObj>(propertyInfo);
            }

            this.PropertyName = propertyInfo.Name;
            this.Info = propertyInfo;
        }

        /// <summary>
        /// 属性赋值操作（Emit技术）
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="value"></param>
        public void SetValue(TObj instance, object value)
        {
            this.setter?.Invoke(instance, value);
        }

        /// <summary>
        /// 属性取值操作(Emit技术)
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public object GetValue(TObj instance)
        {
            return this.getter?.Invoke(instance);
        }

        /// <summary>
        /// 获取对象所有属性
        /// Key：属性名称
        /// Value：属性PropertyEmit实例
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, PropertyEmit<TObj>> GetPropertyEmits()
        {
            return securityCache.GetOrAdd(typeof(TObj), t => t.GetProperties().Select(p => new PropertyEmit<TObj>(p)).ToDictionary(k => k.PropertyName));
        }

        /// <summary>
        /// 获取对象指定属性
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static PropertyEmit<TObj> GetPropertyEmit(string propertyName)
        {
            var propertyEmits = GetPropertyEmits();
            if (!propertyEmits.ContainsKey(propertyName))
            {
                throw new ArgumentException($"类型“{typeof(TObj).FullName}”不含属性“{propertyName}”", nameof(propertyName));
            }
            return propertyEmits[propertyName];
        }

        /// <summary>
        /// 创建属性值MD5签名
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="propertyNames"></param>
        /// <returns></returns>
        public static string CreatePropertyValueMd5Sign(TObj instance, params string[] propertyNames)
        {
            if (instance is null)
            {
                return string.Empty;
            }
            if (propertyNames is null)
            {
                throw new ArgumentNullException(nameof(propertyNames));
            }
            var properties = GetPropertyEmits();
            Dictionary<string, object> propertyValues = new Dictionary<string, object>();
            foreach (var propertyName in propertyNames)
            {
                propertyValues.Add(propertyName, properties[propertyName].GetValue(instance));
            }

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encryptedBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(propertyValues).ToUpper()));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                sb.AppendFormat("{0:x2}", encryptedBytes[i]);
            }
            return sb.ToString();
        }
    }
}
