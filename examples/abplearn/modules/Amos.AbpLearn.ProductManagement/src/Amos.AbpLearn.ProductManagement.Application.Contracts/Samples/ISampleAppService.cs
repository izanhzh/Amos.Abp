using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Amos.AbpLearn.ProductManagement.Samples
{
    public interface ISampleAppService : IApplicationService
    {
        Task<SampleDto> GetAsync();

        Task<SampleDto> GetAuthorizedAsync();
    }
}
