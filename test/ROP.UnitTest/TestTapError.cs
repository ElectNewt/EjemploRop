using System.Threading.Tasks;
using Xunit;

namespace ROP.UnitTest
{
    public class TestTapError : BaseResultTest
    {
        [Fact]
        public void TapError_Action_Is_Called_On_Failure()
        {
            bool called = false;
            Result<string> result = IntToStringFailure(1)
                .TapError(_ => called = true);

            Assert.False(result.Success);
            Assert.True(called);
        }

        [Fact]
        public void TapError_Action_Is_Not_Called_On_Success()
        {
            bool called = false;
            Result<string> result = IntToString(1)
                .TapError(_ => called = true);

            Assert.True(result.Success);
            Assert.False(called);
        }

        [Fact]
        public async Task TapError_Async_Action_Is_Called_On_Failure()
        {
            bool called = false;
            Result<string> result = await IntToStringAsyncFailure(1)
                .TapError(async _ => { called = true; await Task.Yield(); });

            Assert.False(result.Success);
            Assert.True(called);
        }

        [Fact]
        public async Task TapError_Async_Action_Is_Not_Called_On_Success()
        {
            bool called = false;
            Result<string> result = await IntToStringAsync(1)
                .TapError(async _ => { called = true; await Task.Yield(); });

            Assert.True(result.Success);
            Assert.False(called);
        }

        [Fact]
        public async Task TapError_Action_On_Task_Result_Is_Called_On_Failure()
        {
            bool called = false;
            Result<string> result = await IntToStringAsyncFailure(1)
                .TapError(_ => called = true);

            Assert.False(result.Success);
            Assert.True(called);
        }

        [Fact]
        public async Task TapError_Async_Action_On_Task_Result_Is_Called_On_Failure()
        {
            bool called = false;
            Result<string> result = await IntToStringAsyncFailure(1)
                .TapError(async _ => { called = true; await Task.Yield(); });

            Assert.False(result.Success);
            Assert.True(called);
        }
    }
}
