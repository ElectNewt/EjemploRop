using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ROP.UnitTest
{
    public class TestMap : BaseResultTest
    {

        [Fact]
        public void TestMapAllCorrect()
        {
            int originalValue = 1;

            Result<int> result = IntToString(originalValue)
                .Map(MapToInt);

            Assert.True(result.Success);
            Assert.Equal(originalValue, result.Value);
        }

        [Fact]
        public async Task TestMapAsyncAllCorrect()
        {
            int originalValue = 1;

            Result<int> result = await IntToStringAsync(originalValue)
                .Map(MapToIntAsync);

            Assert.True(result.Success);
            Assert.Equal(originalValue, result.Value);
        }
        
        [Fact]
        public async Task TestMapAsyncWithSyncInTheMiddle()
        {
            int originalValue = 1;

            Result<List<string>> result = await IntToStringAsync(originalValue)
                .Map(a=>new List<string>(){a});

            Assert.True(result.Success);
            Assert.Single(result.Value);
            Assert.Equal("1", result.Value.First());
        }

        
        private int MapToInt(string s)
        {
            return Convert.ToInt32(s);
        }
        private Task<int> MapToIntAsync(string s)
        {
            return Task.FromResult(Convert.ToInt32(s));
        }
    }
}
