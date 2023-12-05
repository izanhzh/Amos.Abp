using Amos.Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Amos.AbpLearn.ProductManagement.EntityFrameworkCore
{
    [ConnectionStringName(ProductManagementDbProperties.ConnectionStringName)]
    public class ProductManagementDbContext :
        AbpDbContext<ProductManagementDbContext>,
        IProductManagementDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        public ProductManagementDbContext(DbContextOptions<ProductManagementDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.AutoAddEntityTypeToModel(this);
            builder.AutoAddTempTableToModel(this);

            base.OnModelCreating(builder);

            builder.ConfigureProductManagement();
        }
    }
}