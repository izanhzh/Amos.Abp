using Volo.Abp.Reflection;

namespace Amos.AbpLearn.OrderManagement.Permissions
{
    public class OrderManagementPermissions
    {
        public const string GroupName = "OrderManagement";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(OrderManagementPermissions));
        }
    }
}