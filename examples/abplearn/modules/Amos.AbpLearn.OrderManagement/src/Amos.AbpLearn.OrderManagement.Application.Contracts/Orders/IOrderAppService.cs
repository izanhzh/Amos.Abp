using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Amos.AbpLearn.OrderManagement.Orders
{
    public interface IOrderAppService : IApplicationService
    {
        Task<SampleDto> GetAsync();

        Task<SampleDto> GetAuthorizedAsync();
    }
}
