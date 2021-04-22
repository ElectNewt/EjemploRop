using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ROP.UnitTest
{
    public class TestBind : BaseResultTest
    {

        [Fact]
        public void TestBindAllCorrect()
        {
            int originalValue = 1;

            Result<int> result = IntToString(originalValue)
                .Bind(StringIntoInt);

            Assert.True(result.Success);
            Assert.Equal(originalValue, result.Value);
        }

        [Fact]
        public async Task TestBindAsyncAllCorrect()
        {
            int originalValue = 1;

            Result<int> result = await IntToStringAsync(originalValue)
                .Bind(StringIntoIntAsync);

            Assert.True(result.Success);
            Assert.Equal(originalValue, result.Value);
        }

        [Fact]
        public void TestBindWithFailure()
        {
            int originalValue = 1;

            Result<int> result = IntToString(originalValue)
                .Bind(StringIntoIntFailure);

            Assert.False(result.Success);
            Assert.Equal(default(int), result.Value);
            Assert.Single(result.Errors);
            Assert.Contains("error", result.Errors.First().Message);
            Assert.Equal(HttpStatusCode.NotFound, result.HttpStatusCode);
        }

        [Fact]
        public async Task TestBindWithFailureAsync()
        {
            int originalValue = 1;

            Result<int> result = await IntToStringAsyncFailure(originalValue)
                .Bind(StringIntoIntAsyncFailure);

            Assert.False(result.Success);
            Assert.Equal(default(int), result.Value);
            Assert.Single(result.Errors);
            Assert.Contains("error", result.Errors.First().Message);
        }

    }
}
