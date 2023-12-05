namespace Amos.AbpLearn.OrderManagement
{
    public static class OrderManagementDbProperties
    {
        public static string DbTablePrefix { get; set; } = "OrderManagement";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "OrderManagement";
    }
}
