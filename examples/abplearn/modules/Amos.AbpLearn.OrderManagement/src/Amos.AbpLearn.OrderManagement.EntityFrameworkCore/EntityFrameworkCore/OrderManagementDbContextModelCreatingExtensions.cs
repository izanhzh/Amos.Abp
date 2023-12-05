using System;
using Amos.AbpLearn.OrderManagement.Orders;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Amos.AbpLearn.OrderManagement.EntityFrameworkCore
{
    public static class OrderManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureOrderManagement(
            this ModelBuilder builder,
            Action<OrderManagementModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new OrderManagementModelBuilderConfigurationOptions(
                OrderManagementDbProperties.DbTablePrefix,
                OrderManagementDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            /* Configure all entities here. Example:

            builder.Entity<Question>(b =>
            {
                //Configure table & schema name
                b.ToTable(options.TablePrefix + "Questions", options.Schema);
            
                b.ConfigureByConvention();
            
                //Properties
                b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);
                
                //Relations
                b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

                //Indexes
                b.HasIndex(q => q.CreationTime);
            });
            */

            builder.Entity<Order>(b =>
            {
                //Configure table & schema name
                b.ToTable(options.TablePrefix + "Order", options.Schema);

                b.ConfigureByConvention();
            });
        }
    }
}