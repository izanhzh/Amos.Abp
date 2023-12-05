using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Linq;

namespace Amos.AbpLearn.OrderManagement.Orders
{
        public class OrderAppService : OrderManagementAppService, IOrderAppService
        {
            private readonly Lazy<IRepository<Order, long>> _orderRepositoryLazy;
            private readonly Lazy<IRepository<IdentityUser>> _identityUsersRepositoryLazy;
            private readonly Lazy<IAsyncQueryableExecuter> _asyncExecuterLazy;//https://docs.abp.io/en/abp/latest/Repositories  IQueryable & Async Operations

            public OrderAppService(
                Lazy<IRepository<Order, long>> orderRepositoryLazy,
                Lazy<IRepository<IdentityUser>> identityUsersRepositoryLazy,
                Lazy<IAsyncQueryableExecuter> asyncExecuterLazy)
            {
                _orderRepositoryLazy = orderRepositoryLazy;
                _identityUsersRepositoryLazy = identityUsersRepositoryLazy;
                _asyncExecuterLazy = asyncExecuterLazy;
            }

        public async Task<SampleDto> GetAsync()
        {
            try
            {
                var query1 = await _orderRepositoryLazy.Value.GetQueryableAsync();
                var count1 = await _asyncExecuterLazy.Value.CountAsync(query1);

                var query2 = await _identityUsersRepositoryLazy.Value.GetQueryableAsync();
                var count2 = await _asyncExecuterLazy.Value.CountAsync(query2);
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
        public Task<SampleDto> GetAuthorizedAsync()
        {
            return Task.FromResult(
                new SampleDto
                {
                    Value = 42
                }
            );
        }
    }
}