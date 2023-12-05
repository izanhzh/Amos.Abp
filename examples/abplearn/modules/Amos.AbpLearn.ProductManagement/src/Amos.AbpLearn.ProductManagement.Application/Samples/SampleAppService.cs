using Amos.AbpLearn.ProductManagement.Products;
using Amos.AbpLearn.ProductManagement.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Linq;

namespace Amos.AbpLearn.ProductManagement.Samples
{
    public class SampleAppService : ProductManagementAppService, ISampleAppService
    {
        private readonly Lazy<IRepository<Product, long>> _productRepositoryLazy;
        private readonly Lazy<IRepository<IdentityUser>> _identityUsersRepositoryLazy;
        private readonly Lazy<IAsyncQueryableExecuter> _asyncExecuterLazy;//https://docs.abp.io/en/abp/latest/Repositories  IQueryable & Async Operations
        private readonly Lazy<IProductManagementTempTableRepository> _productManagementTempTableRepositoryLazy;
        private readonly Lazy<IProductManagementSqlScriptRepository> _productManagementSqlScriptRepositoryLazy;

        public SampleAppService(
          Lazy<IRepository<Product, long>> productRepositoryLazy,
          Lazy<IRepository<IdentityUser>> identityUsersRepositoryLazy,
          Lazy<IAsyncQueryableExecuter> asyncExecuterLazy,
          Lazy<IProductManagementTempTableRepository> productManagementTempTableRepositoryLazy,
          Lazy<IProductManagementSqlScriptRepository> productManagementSqlScriptRepositoryLazy)
        {
            _productRepositoryLazy = productRepositoryLazy;
            _identityUsersRepositoryLazy = identityUsersRepositoryLazy;
            _asyncExecuterLazy = asyncExecuterLazy;
            _productManagementTempTableRepositoryLazy = productManagementTempTableRepositoryLazy;
            _productManagementSqlScriptRepositoryLazy = productManagementSqlScriptRepositoryLazy;
        }

        public async Task<SampleDto> GetAsync()
        {
            try
            {
                var sqlQuery = await _productManagementSqlScriptRepositoryLazy.Value.QueryAsDataSetAsync("ProductManagement:TestMultiline", sqlParam: new { Id = 1 });
                var sqlQueryResult = sqlQuery;

                var a1 = await _productManagementSqlScriptRepositoryLazy.Value.QueryAsDataTableAsync("ProductManagement:SelectDynamicTableById",new { table= "OrderManagementOrder" }, new { Id = 1 });

                var tempDatas = new List<MyFristTempTable> { new MyFristTempTable { Id = 1 }, new MyFristTempTable { Id = 2 } };
                var tempQuery = await _productManagementTempTableRepositoryLazy.Value.InsertIntoTempTableAsync(tempDatas);

                var a = await _asyncExecuterLazy.Value.CountAsync(tempQuery);


                var query1 = await _productRepositoryLazy.Value.GetQueryableAsync();

                var tempWhereQuery = query1.Where(w => tempQuery.Any(a => a.Id == w.Id));
                var tempWhereResult = await _productRepositoryLazy.Value.AsyncExecuter.ToListAsync(tempWhereQuery);

                //var products = await _asyncExecuterLazy.Value.ToListAsync(query1);
                //foreach (var product in products)
                //{
                //    product.Description = "测试" + DateTime.Now;
                //}
                //await _productRepositoryLazy.Value.UpdateManyAsync(products);

                var c = query1.Count();
                var count = await _asyncExecuterLazy.Value.CountAsync(query1);

                var query2 = await _identityUsersRepositoryLazy.Value.GetQueryableAsync();

                var query3 = query1
                    .Join(query2, o => o.CreatorId, i => i.Id, (r1, r2) => new
                    {
                        Product = r1,
                        Creator = r2
                    });


                var r = await _asyncExecuterLazy.Value.ToListAsync(query3);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
            }

            return await Task.FromResult(
                new SampleDto
                {
                    Value = 42
                }
            );
        }

        [Authorize]
        public async Task<SampleDto> GetAuthorizedAsync()
        {
            try
            {
                //await _productRepositoryLazy.Value.InsertAsync(new Product { Name = "Test1 By" + CurrentUser.UserName, Description = "测试" });

                Logger.LogInformation(CurrentUser.UserName);

                var query1 = await _productRepositoryLazy.Value.GetQueryableAsync();
                var count = await _asyncExecuterLazy.Value.CountAsync(query1);

                var query2 = await _identityUsersRepositoryLazy.Value.GetQueryableAsync();

                var query3 = query1
                    .Join(query2, o => o.CreatorId, i => i.Id, (r1, r2) => new
                    {
                        Product = r1,
                        Creator = r2
                    });


                var r = await _asyncExecuterLazy.Value.ToListAsync(query3);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
            }

            return await Task.FromResult(
                new SampleDto
                {
                    Value = 42
                }
            );
        }
    }
}