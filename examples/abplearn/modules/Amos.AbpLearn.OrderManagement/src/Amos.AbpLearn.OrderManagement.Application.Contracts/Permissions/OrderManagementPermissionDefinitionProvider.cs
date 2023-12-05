using Amos.AbpLearn.OrderManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Amos.AbpLearn.OrderManagement.Permissions
{
    public class OrderManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(OrderManagementPermissions.GroupName, L("Permission:OrderManagement"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<OrderManagementResource>(name);
        }
    }
}