using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace Amos.Abp.Extensions.ObjectExtension
{
    public static class PropertyEmitExtension
    {
        /// <summary>
        /// 获取对象所有属性
        /// Key：属性名称
        /// Value：属性PropertyEmit实例
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, PropertyEmit<TObj>> GetPropertyEmits<TObj>(this TObj _)
        {
            return PropertyEmit<TObj>.GetPropertyEmits();
        }

        /// <summary>
        /// 获取对象指定属性
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static PropertyEmit<TObj> GetPropertyEmit<TObj>(this TObj _, string propertyName)
        {
            return PropertyEmit<TObj>.GetPropertyEmit(propertyName);
        }

        /// <summary>
        /// 创建属性值MD5签名
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="propertyNames"></param>
        /// <returns></returns>
        public static string CreatePropertyValueMd5Sign<TObj>(this TObj instance, params string[] propertyNames)
        {
            return PropertyEmit<TObj>.CreatePropertyValueMd5Sign(instance, propertyNames);
        }

        /// <summary>
        /// 根据表达式获取属性名称
        /// </summary>
        /// <typeparam name="TObj"></typeparam>
        /// <param name="_"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string[] GetPropertyNameByExpression<TObj>(this TObj _, Expression<Func<TObj, object>> expression)
        {
            return GetPropertyNameByExpression(expression);
        }

        /// <summary>
        /// 根据表达式获取属性名称
        /// </summary>
        /// <typeparam name="TObj"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string[] GetPropertyNameByExpression<TObj>(Expression<Func<TObj, object>> expression)
        {
            if (expression is null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
            switch (expression.Body.NodeType)
            {
                case ExpressionType.New:
                    var newExpression = ((NewExpression)expression.Body);
                    var members = newExpression.Members.Select(s => s.Name).ToArray();
                    var arguments = newExpression.Arguments.Select(s => ((MemberExpression)s).Member.Name).ToArray();
                    if (members.Except(arguments).Any() || arguments.Except(members).Any())
                    {
                        return null;
                    }
                    return members;
                case ExpressionType.Convert:
                    var operand = ((UnaryExpression)expression.Body).Operand;
                    if (operand.NodeType != ExpressionType.MemberAccess)
                    {
                        return null;
                    }
                    return new string[] { ((MemberExpression)operand).Member.Name };
                case ExpressionType.MemberAccess:
                    return new string[] { ((MemberExpression)expression.Body).Member.Name };
                default:
                    return null;
            }
        }

        /// <summary>
        /// 比较两个对象指定的属性是否相等（注意：使用前请仔细阅读<see cref="ObjectPropertyEqualityComparerOptions"/>查看默认的比较行为）
        /// </summary>
        /// <typeparam name="TObj"></typeparam>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <param name="compareProperties"></param>
        /// <returns></returns>
        public static bool Equals<TObj>(this TObj obj1, TObj obj2, params string[] compareProperties) where TObj : class
        {
            return new ObjectPropertyEqualityComparer<TObj>(compareProperties).Equals(obj1, obj2);
        }

        /// <summary>
        /// 比较两个对象指定的属性是否相等
        /// </summary>
        /// <typeparam name="TObj"></typeparam>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <param name="configureOptions"></param>
        /// <param name="compareProperties"></param>
        /// <returns></returns>
        public static bool Equals<TObj>(this TObj obj1, TObj obj2, Action<ObjectPropertyEqualityComparerOptions> configureOptions, params string[] compareProperties) where TObj : class
        {
            return new ObjectPropertyEqualityComparer<TObj>(configureOptions, compareProperties).Equals(obj1, obj2);
        }

        /// <summary>
        /// 比较两个对象指定的属性是否相等（注意：使用前请仔细阅读<see cref="ObjectPropertyEqualityComparerOptions"/>查看默认的比较行为）
        /// </summary>
        /// <typeparam name="TObj"></typeparam>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <param name="comparePropertiesSelector"></param>
        /// <returns></returns>
        public static bool Equals<TObj>(this TObj obj1, TObj obj2, Expression<Func<TObj, object>> comparePropertiesSelector) where TObj : class
        {
            return new ObjectPropertyEqualityComparer<TObj>(comparePropertiesSelector).Equals(obj1, obj2);
        }

        /// <summary>
        /// 比较两个对象指定的属性是否相等
        /// </summary>
        /// <typeparam name="TObj"></typeparam>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <param name="configureOptions"></param>
        /// <param name="comparePropertiesSelector"></param>
        /// <returns></returns>
        public static bool Equals<TObj>(this TObj obj1, TObj obj2, Action<ObjectPropertyEqualityComparerOptions> configureOptions, Expression<Func<TObj, object>> comparePropertiesSelector) where TObj : class
        {
            return new ObjectPropertyEqualityComparer<TObj>(configureOptions, comparePropertiesSelector).Equals(obj1, obj2);
        }

        /// <summary>
        /// 根据指定属性对集合去重（注意：使用前请仔细阅读<see cref="ObjectPropertyEqualityComparerOptions"/>查看默认的比较行为）
        /// </summary>
        /// <typeparam name="TObj"></typeparam>
        /// <param name="source"></param>
        /// <param name="compareProperties"></param>
        /// <returns></returns>
        public static IEnumerable<TObj> Distinct<TObj>(this IEnumerable<TObj> source, params string[] compareProperties) where TObj : class
        {
            return source.Distinct(new ObjectPropertyEqualityComparer<TObj>(compareProperties));
        }

        /// <summary>
        /// 根据指定属性对集合去重
        /// </summary>
        /// <typeparam name="TObj"></typeparam>
        /// <param name="source"></param>
        /// <param name="configureOptions"></param>
        /// <param name="compareProperties"></param>
        /// <returns></returns>
        public static IEnumerable<TObj> Distinct<TObj>(this IEnumerable<TObj> source, Action<ObjectPropertyEqualityComparerOptions> configureOptions, params string[] compareProperties) where TObj : class
        {
            return source.Distinct(new ObjectPropertyEqualityComparer<TObj>(configureOptions, compareProperties));
        }

        /// <summary>
        /// 根据指定属性对集合去重（注意：使用前请仔细阅读<see cref="ObjectPropertyEqualityComparerOptions"/>查看默认的比较行为）
        /// </summary>
        /// <typeparam name="TObj"></typeparam>
        /// <param name="source"></param>
        /// <param name="comparePropertiesSelector"></param>
        /// <returns></returns>
        public static IEnumerable<TObj> Distinct<TObj>(this IEnumerable<TObj> source, Expression<Func<TObj, object>> comparePropertiesSelector) where TObj : class
        {
            return source.Distinct(new ObjectPropertyEqualityComparer<TObj>(comparePropertiesSelector));
        }

        /// <summary>
        /// 根据指定属性对集合去重
        /// </summary>
        /// <typeparam name="TObj"></typeparam>
        /// <param name="source"></param>
        /// <param name="configureOptions"></param>
        /// <param name="comparePropertiesSelector"></param>
        /// <returns></returns>
        public static IEnumerable<TObj> Distinct<TObj>(this IEnumerable<TObj> source, Action<ObjectPropertyEqualityComparerOptions> configureOptions, Expression<Func<TObj, object>> comparePropertiesSelector) where TObj : class
        {
            return source.Distinct(new ObjectPropertyEqualityComparer<TObj>(configureOptions, comparePropertiesSelector));
        }

        /// <summary>
        /// 比较两个对象指定的属性是否相等，如果不想等则将source的属性值更新成与target的一样（注意：使用前请仔细阅读<see cref="ObjectPropertyEqualityComparerOptions"/>查看默认的比较行为）
        /// </summary>
        /// <typeparam name="TObj"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="compareProperties"></param>
        /// <returns></returns>
        public static bool TryUpdatePropertyValueIfNotEquals<TObj>([DisallowNull] this TObj source, [DisallowNull] TObj target, params string[] compareProperties) where TObj : class
        {
            return new ObjectPropertyEqualityComparer<TObj>(compareProperties).TryUpdatePropertyValueIfNotEquals(source, target);
        }

        /// <summary>
        /// 比较两个对象指定的属性是否相等，如果不想等则将source的属性值更新成与target的一样
        /// </summary>
        /// <typeparam name="TObj"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="configureOptions"></param>
        /// <param name="compareProperties"></param>
        /// <returns></returns>
        public static bool TryUpdatePropertyValueIfNotEquals<TObj>([DisallowNull] this TObj source, [DisallowNull] TObj target, Action<ObjectPropertyEqualityComparerOptions> configureOptions, params string[] compareProperties) where TObj : class
        {
            return new ObjectPropertyEqualityComparer<TObj>(configureOptions, compareProperties).TryUpdatePropertyValueIfNotEquals(source, target);
        }

        /// <summary>
        /// 比较两个对象指定的属性是否相等，如果不想等则将source的属性值更新成与target的一样（注意：使用前请仔细阅读<see cref="ObjectPropertyEqualityComparerOptions"/>查看默认的比较行为）
        /// </summary>
        /// <typeparam name="TObj"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="comparePropertiesSelector"></param>
        /// <returns></returns>
        public static bool TryUpdatePropertyValueIfNotEquals<TObj>([DisallowNull] this TObj source, [DisallowNull] TObj target, Expression<Func<TObj, object>> comparePropertiesSelector) where TObj : class
        {
            return new ObjectPropertyEqualityComparer<TObj>(comparePropertiesSelector).TryUpdatePropertyValueIfNotEquals(source, target);
        }

        /// <summary>
        /// 比较两个对象指定的属性是否相等，如果不想等则将source的属性值更新成与target的一样
        /// </summary>
        /// <typeparam name="TObj"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="configureOptions"></param>
        /// <param name="comparePropertiesSelector"></param>
        /// <returns></returns>
        public static bool TryUpdatePropertyValueIfNotEquals<TObj>([DisallowNull] this TObj source, [DisallowNull] TObj target, Action<ObjectPropertyEqualityComparerOptions> configureOptions, Expression<Func<TObj, object>> comparePropertiesSelector) where TObj : class
        {
            return new ObjectPropertyEqualityComparer<TObj>(configureOptions, comparePropertiesSelector).TryUpdatePropertyValueIfNotEquals(source, target);
        }
    }
}
