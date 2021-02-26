using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                .Then(x => StringIntoInt(x));

            Assert.True(result.Success);
            Assert.Equal(originalValue.ToString(), result.Value);
        }
        [Fact]
        public async Task TestThenAsyncGetsIgnoredForTheResult()
        {
            int originalValue = 1;

            Result<string> result = await originalValue.Success().Async()
                .Bind(IntToStringAsync)
                .Then(x => StringIntoIntAsync(x));

            Assert.True(result.Success);
            Assert.Equal(originalValue.ToString(), result.Value);
        }
    }
}
