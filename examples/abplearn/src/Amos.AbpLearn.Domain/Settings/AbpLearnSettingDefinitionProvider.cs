using Volo.Abp.Settings;

namespace Amos.AbpLearn.Settings
{
    public class AbpLearnSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(AbpLearnSettings.MySetting1));
        }
    }
}
