using System.Threading.Tasks;
using Xunit;

namespace ROP.UnitTest
{
    public class TestIgnore : BaseResultTest
    {
        [Fact]
        public void TestIgnoreGetsIgnored()
        {
            var result = 1.Success().Ignore();

            Assert.IsType<Result<Unit>>(result);
        }

        [Fact]
        public async Task TestIgnoreGetsIgnoredAsync()
        {
            var result = await 1.Success().Async().Ignore();

            Assert.IsType<Result<Unit>>(result);
        }
    }
}
