using System;
using System.Globalization;

namespace Amos.Abp.Extensions
{
    public static class ConvertExtension
    {
        /// <summary>
        /// 判断object类型值是否是null或者DBNull
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool IsNullOrDBNull(this object value)
        {
            if (value == null || Convert.IsDBNull(value))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 将object类型值转换为bool类型值，如果无法转换，则返回false
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBooleanEx(this object value)
        {
            return value.ToStringEx().ToBooleanEx();
        }

        /// <summary>
        /// 将string类型值转换为bool类型值（1会转换成true），如果无法转换，则返回false
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBooleanEx(this string value)
        {
            bool result = true;
            if (value != "1" && !bool.TryParse(value, out result))
            {
                return false;
            }
            return result;
        }

        /// <summary>
        /// 将object类型值转换为DateTime类型值，如果无法转换，则返回DateTime默认值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToDateTimeEx(this object value)
        {
            return value.ToStringEx().ToDateTimeEx();
        }

        /// <summary>
        /// 将string类型值转换为DateTime类型值，如果无法转换，则返回DateTime默认值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToDateTimeEx(this string value)
        {
            DateTime result;
            if (!DateTime.TryParse(value, out result))
            {
                return default(DateTime);
            }
            return result;
        }

        /// <summary>
        /// 将object类型值转换为Decimal类型值，如果无法转换，则返回0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ToDecimalEx(this object value)
        {
            return value.ToStringEx().ToDecimalEx();
        }

        /// <summary>
        /// 将string类型值转换为Decimal类型值，如果无法转换，则返回0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ToDecimalEx(this string value)
        {
            decimal result = 0;
            if (!decimal.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out result))
            {
                return 0;
            }
            return result;
        }

        /// <summary>
        /// 将object类型值转换为double类型值，如果无法转换，则返回0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDoubleEx(this object value)
        {
            return value.ToStringEx().ToDoubleEx();
        }

        /// <summary>
        /// 将string类型值转换为double类型值，如果无法转换，则返回0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDoubleEx(this string value)
        {
            double result = 0;
            if (!double.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out result))
            {
                return 0;
            }
            return result;
        }

        /// <summary>
        /// 将object类型值转换为Guid类型值，如果无法转换，则返回Guid默认值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Guid ToGuid(this object value)
        {
            return value.ToStringEx().ToGuid();
        }

        /// <summary>
        /// 将string类型值转换为Guid类型值，如果无法转换，则返回Guid默认值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Guid ToGuid(this string value)
        {
            Guid result;
            if (!Guid.TryParse(value, out result))
            {
                return default(Guid);
            }
            return result;
        }

        /// <summary>
        /// 将object类型值转换为short类型值，如果无法转换，则返回0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static short ToInt16Ex(this object value)
        {
            return value.ToStringEx().ToInt16Ex();
        }

        /// <summary>
        /// 将string类型值转换为short类型值，如果无法转换，则返回0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static short ToInt16Ex(this string value)
        {
            short result = 0;
            if (!short.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out result))
            {
                return 0;
            }
            return result;
        }

        /// <summary>
        /// 将object类型值转换为int类型值，如果无法转换，则返回0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt32Ex(this object value)
        {
            return value.ToStringEx().ToInt32Ex();
        }

        /// <summary>
        /// 将string类型值转换为int类型值，如果无法转换，则返回0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt32Ex(this string value)
        {
            int result = 0;
            if (!int.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out result))
            {
                return 0;
            }
            return result;
        }

        /// <summary>
        /// 将object类型值转换为long类型值，如果无法转换，则返回0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ToInt64Ex(this object value)
        {
            return value.ToStringEx().ToInt64Ex();
        }

        /// <summary>
        /// 将string类型值转换为long类型值，如果无法转换，则返回0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ToInt64Ex(this string value)
        {
            long result = 0;
            if (!long.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out result))
            {
                return 0;
            }
            return result;
        }

        /// <summary>
        /// 将object类型值转换为string类型值，如果object是DBNull或null，则返回空字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToStringEx(this object value)
        {
            return value.IsNullOrDBNull() ? "" : value.ToString();
        }

        /// <summary>
        /// 将object类型值转换为float类型值，如果无法转换，则返回0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float ToSingleEx(this object value)
        {
            return value.ToStringEx().ToSingleEx();
        }

        /// <summary>
        /// 将string类型值转换为float类型值，如果无法转换，则返回0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float ToSingleEx(this string value)
        {
            float result = 0;
            if (!float.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out result))
            {
                return 0;
            }
            return result;
        }
    }
}
