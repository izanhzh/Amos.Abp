using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Amos.AbpLearn.ProductManagement.Products
{
    public class ProductDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
