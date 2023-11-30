using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Amos.Abp.Extensions.ObjectExtension
{
    /// <summary>
    /// 对象属性值相等比较
    /// </summary>
    public class ObjectPropertyEqualityComparer<T> : IEqualityComparer<T>
        where T : class
    {
        private readonly string[] _compareProperties;
        private readonly ObjectPropertyEqualityComparerOptions _options = new ObjectPropertyEqualityComparerOptions();
        private readonly Dictionary<string, PropertyEmit<T>> _propertyEmitDic = PropertyEmit<T>.GetPropertyEmits();

        public ObjectPropertyEqualityComparer(params string[] compareProperties)
        {
            if (compareProperties is null)
            {
                throw new ArgumentNullException(nameof(compareProperties));
            }
            foreach (var property in compareProperties)
            {
                if (!_propertyEmitDic.ContainsKey(property) || !_propertyEmitDic[property].Info.CanRead)
                {
                    throw new ArgumentException($"类型“{typeof(T).FullName}”不含可读属性“{property}”", nameof(compareProperties));
                }
            }
            _compareProperties = compareProperties;
        }

        public ObjectPropertyEqualityComparer(Action<ObjectPropertyEqualityComparerOptions> configureOptions, params string[] compareProperties)
            : this(compareProperties)
        {
            if (configureOptions is null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }
            configureOptions(_options);
        }

        public ObjectPropertyEqualityComparer(Expression<Func<T, object>> comparePropertiesSelector)
        {
            var compareProperties = PropertyEmitExtension.GetPropertyNameByExpression(comparePropertiesSelector);
            if (compareProperties is null)
            {
                throw new ArgumentNullException(nameof(compareProperties));
            }
            foreach (var property in compareProperties)
            {
                if (!_propertyEmitDic.ContainsKey(property) || !_propertyEmitDic[property].Info.CanRead)
                {
                    throw new ArgumentException($"类型“{typeof(T).FullName}”不含可读属性“{property}”", nameof(compareProperties));
                }
            }
            _compareProperties = compareProperties;
        }

        public ObjectPropertyEqualityComparer(Action<ObjectPropertyEqualityComparerOptions> configureOptions, Expression<Func<T, object>> comparePropertiesSelector)
            : this(comparePropertiesSelector)
        {
            if (configureOptions is null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }
            configureOptions(_options);
        }

        public bool Equals([AllowNull] T x, [AllowNull] T y)
        {
            if (x == null || y == null)
            {
                return x == y;
            }

            foreach (var property in _compareProperties)
            {
                if (!IsEqualsOfProperty(x, y, property))
                {
                    return false;
                }
            }

            return true;
        }

        public virtual bool TryUpdatePropertyValueIfNotEquals([DisallowNull] T source, [DisallowNull] T target)
        {
            var isUpdated = false;
            foreach (var property in _compareProperties)
            {
                if (!IsEqualsOfProperty(source, target, property))
                {
                    _propertyEmitDic[property].SetValue(source, _propertyEmitDic[property].GetValue(target));
                    isUpdated = true;
                }
            }
            return isUpdated;
        }

        public int GetHashCode([DisallowNull] T obj)
        {
            var hashCode = 0;
            foreach (var property in _compareProperties)
            {
                var value = GetPropertyValue(obj, property);
                hashCode ^= value?.GetHashCode() ?? 0;
            }
            return hashCode;
        }

        protected virtual bool IsEqualsOfProperty(T x, T y, string property)
        {
            var value1 = GetPropertyValue(x, property);
            var value2 = GetPropertyValue(y, property);

            if (value1 == null || value2 == null)
            {
                return value1 == value2;
            }
            return value1.Equals(value2);
        }

        private object GetPropertyValue(T obj, string property)
        {
            var value = obj == null ? null : _propertyEmitDic[property].GetValue(obj);
            if (_propertyEmitDic[property].Info.PropertyType == typeof(string))
            {
                var str = (string)value;
                if (_options.StringPropertyAutoConvertNullToEmpty)
                {
                    str ??= string.Empty;
                }
                if (_options.StringPropertyAutoTrimWhitespace)
                {
                    str = str?.Trim();
                }
                if (_options.StringPropertyAutoIgnoreCase)
                {
                    str = str?.ToUpper();
                }
                return str;
            }
            else
            {
                return value;
            }
        }
    }
}
