using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace Amos.AbpLearn.OrderManagement.Orders
{
    public class Order : FullAuditedAggregateRoot<long>
    {
        public virtual string CustomerId { get; set; }

        [DataType(DataType.Date)]
        [Display(Description = "下单时间")]
        public virtual DateTime OrderDate { get; set; }
    }
}
