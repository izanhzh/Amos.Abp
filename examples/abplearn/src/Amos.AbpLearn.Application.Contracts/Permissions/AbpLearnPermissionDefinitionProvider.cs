using Amos.AbpLearn.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Amos.AbpLearn.Permissions
{
    public class AbpLearnPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(AbpLearnPermissions.GroupName);
            //Define your own permissions here. Example:
            //myGroup.AddPermission(AbpLearnPermissions.MyPermission1, L("Permission:MyPermission1"));
            myGroup.AddPermission(AbpLearnPermissions.AbpLearnApiSwagger);
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AbpLearnResource>(name);
        }
    }
}
