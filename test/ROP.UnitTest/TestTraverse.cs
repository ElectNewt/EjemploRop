using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ROP.UnitTest
{
    public class TestTraverse 
    {

        [Fact]
        public void TestTraverseConvertListResultIntoResultList()
        {

            Result<List<int>> result = GetResultList()
                .Traverse();

            Assert.True(result.Success);
            Assert.Equal(10, result.Value.Count);
        }

        [Fact]
        public async Task TestTraverseConvertListResultIntoResultListAsync()
        {

            Result<List<int>> result = await GetResultListAsync()
                .Traverse();

            Assert.True(result.Success);
            Assert.Equal(10, result.Value.Count);
        }

        private List<Result<int>> GetResultList()
        {
            List<Result<int>> list = new List<Result<int>>();
            for (int i = 0; i <10; i++)
            {
                list.Add(i.Success());
            }
            return list;
        }

        private List<Task<Result<int>>> GetResultListAsync()
        {
            List<Task<Result<int>>> list = new List<Task<Result<int>>>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(i.Success().Async());
            }
            return list;
        }
    }
}
