namespace Amos.AbpLearn.ProductManagement
{
    public static class ProductManagementDbProperties
    {
        public static string DbTablePrefix { get; set; } = "ProductManagement";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "ProductManagement";
    }
}
