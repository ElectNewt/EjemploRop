using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ROP.UnitTest
{
    public class TestCombine : BaseResultTest
    {
        [Fact]
        public void TestCombineAllCorrect()
        {
            int originalValue = 1;

            Result<(string, int)> result = IntToString(originalValue)
                .Combine(StringIntoInt);
            
            Assert.True(result.Success);
            Assert.Equal(originalValue.ToString(), result.Value.Item1);
            Assert.Equal(originalValue, result.Value.Item2);
        }

        [Fact]
        public async Task TestCombineAsyncAllCorrect()
        {
            int originalValue = 1;

            Result<(string, int)> result = await IntToStringAsync(originalValue)
                .Combine(StringIntoIntAsync);

            Assert.True(result.Success);
            Assert.Equal(originalValue.ToString(), result.Value.Item1);
            Assert.Equal(originalValue, result.Value.Item2);
        }


        [Fact]
        public void TestCombineWithFailure()
        {
            int originalValue = 1;

            Result<(string, int)> result = IntToString(originalValue)
                .Combine(StringIntoIntFailure);

            Assert.False(result.Success);
            Assert.Single(result.Errors);
            Assert.Contains("error", result.Errors.First().Message);
        }

        [Fact]
        public async Task TestCombineWithFailureAsync()
        {
            int originalValue = 1;

            Result<(string, int)> result = await IntToStringAsyncFailure(originalValue)
                .Combine(StringIntoIntAsyncFailure);

            Assert.False(result.Success);
            Assert.Single(result.Errors);
            Assert.Contains("error", result.Errors.First().Message);
        }

        [Fact]
        public async Task TestCombineWithFaiulreMapsToCorrectError()
        {
            int originalValue = 1;

            Result<bool> result = await StringIntoIntAsyncFailure(originalValue.ToString())
                .Combine(IntToStringAsync)
                .Map(_=>true);
            
            Assert.False(result.Success);
            Assert.Single(result.Errors);
            Assert.Equal(HttpStatusCode.NotFound, result.HttpStatusCode);
        }

        [Fact]
        public async Task TestCombineWithFaiulreMapsToCorrectError2()
        {
            int originalValue = 1;

            Result<bool> result = await IntToStringAsync(originalValue)
                .Combine(StringIntoIntAsyncFailure)
                .Map(_ => true);

            Assert.False(result.Success);
            Assert.Single(result.Errors);
            Assert.Equal(HttpStatusCode.NotFound, result.HttpStatusCode);
        }

        [Fact]
        public async Task TestCombindWithNonAsyncMethodInTheMiddle()
        {
            int originalValue = 1;

            Result<string> result = await IntToStringAsync(originalValue)   // <- async value
                .Combine(StringIntoInt)                                     // <- Sincronous method
                .Bind(x => x.Item1.Success().Async());                      // <- async metohd

            Assert.True(result.Success);
            Assert.Equal(originalValue.ToString(), result.Value);
        }
    }
}
