using Amos.AbpLearn.OrderManagement.Orders;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Amos.AbpLearn.OrderManagement.EntityFrameworkCore
{
    [ConnectionStringName(OrderManagementDbProperties.ConnectionStringName)]
    public interface IOrderManagementDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */

        DbSet<Order> Orders { get; }
    }
}