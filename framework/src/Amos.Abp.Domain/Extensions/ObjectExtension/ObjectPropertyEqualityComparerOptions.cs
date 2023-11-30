namespace Amos.Abp.Extensions.ObjectExtension
{
    /// <summary>
    /// 对象属性值相等比较配置选项
    /// </summary>
    public class ObjectPropertyEqualityComparerOptions
    {
        /// <summary>
        /// 字符串忽略大小写比较
        /// </summary>
        public bool StringPropertyAutoIgnoreCase { get; set; } = true;

        /// <summary>
        /// 字符串自动去除前后空格
        /// </summary>
        public bool StringPropertyAutoTrimWhitespace { get; set; } = true;

        /// <summary>
        /// 字符串自动将null转换为string.Empty
        /// </summary>
        public bool StringPropertyAutoConvertNullToEmpty { get; set; } = true;
    }
}
