using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace Amos.AbpLearn.ProductManagement.Web.Menus
{
    public class ProductManagementMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenuAsync(context);
            }
        }

        private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            //Add main menu items.
            context.Menu.AddItem(new ApplicationMenuItem(ProductManagementMenus.Prefix, displayName: "ProductManagement", "~/ProductManagement", icon: "fa fa-globe"));

            return Task.CompletedTask;
        }
    }
}