using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Amos.AbpLearn.OrderManagement.Orders
{
    public class SampleAppService_Tests : OrderManagementApplicationTestBase
    {
        private readonly IOrderAppService _sampleAppService;

        public SampleAppService_Tests()
        {
            _sampleAppService = GetRequiredService<IOrderAppService>();
        }

        [Fact]
        public async Task GetAsync()
        {
            var result = await _sampleAppService.GetAsync();
            result.Value.ShouldBe(42);
        }

        [Fact]
        public async Task GetAuthorizedAsync()
        {
            var result = await _sampleAppService.GetAuthorizedAsync();
            result.Value.ShouldBe(42);
        }
    }
}
