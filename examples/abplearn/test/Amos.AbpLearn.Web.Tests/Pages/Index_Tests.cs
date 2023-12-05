using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Amos.AbpLearn.Pages
{
    public class Index_Tests : AbpLearnWebTestBase
    {
        [Fact]
        public async Task Welcome_Page()
        {
            var response = await GetResponseAsStringAsync("/");
            response.ShouldNotBeNull();
        }
    }
}
