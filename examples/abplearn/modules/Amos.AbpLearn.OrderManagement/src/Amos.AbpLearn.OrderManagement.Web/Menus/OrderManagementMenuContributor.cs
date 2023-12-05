using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace Amos.AbpLearn.OrderManagement.Web.Menus
{
    public class OrderManagementMenuContributor : IMenuContributor
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
            context.Menu.AddItem(new ApplicationMenuItem(OrderManagementMenus.Prefix, displayName: "OrderManagement", "~/OrderManagement", icon: "fa fa-globe"));

            return Task.CompletedTask;
        }
    }
}