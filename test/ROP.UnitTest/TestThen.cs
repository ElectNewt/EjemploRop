using System.Threading.Tasks;
using Xunit;

namespace ROP.UnitTest
{
    public class TestThen : BaseResultTest
    {
        [Fact]
        public void TestThenGetsIgnoredForTheResult()
        {
            int originalValue = 1;

            Result<string> result =
                originalValue.Success()
                .Bind(IntToString)
                .Then(StringIntoInt);

            Assert.True(result.Success);
            Assert.Equal(originalValue.ToString(), result.Value);
        }
        
        [Fact]
        public async Task TestThenAsyncGetsIgnoredForTheResult()
        {
            int originalValue = 1;

            Result<string> result = await originalValue.Success().Async()
                .Bind(IntToStringAsync)
                .Then(StringIntoIntAsync);

            Assert.True(result.Success);
            Assert.Equal(originalValue.ToString(), result.Value);
        }

        [Fact]
        public async Task TestThenWithNonAsyncMethodInTheMiddleGetsIgnoredForTheResult()
        {
            int originalValue = 1;

            Result<string> result = await originalValue.Success().Async()   // <- async value
                .Then(IntToString)                                          // <- Sincronous method
                .Bind(IntToStringAsync)                                     // <- async metohd
                .Then(StringIntoIntAsync);

            Assert.True(result.Success);
            Assert.Equal(originalValue.ToString(), result.Value);
        }
    }
}