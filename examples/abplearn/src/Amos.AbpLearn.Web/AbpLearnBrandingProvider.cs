using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Amos.AbpLearn.Web
{
    [Dependency(ReplaceServices = true)]
    public class AbpLearnBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "AbpLearn";
    }
}
