using System.Threading.Tasks;
using Xunit;

namespace ROP.UnitTest
{
    public class TestTap : BaseResultTest
    {
        [Fact]
        public void Tap_Action_Is_Called_On_Success()
        {
            int originalValue = 1;
            bool called = false;

            Result<string> result = IntToString(originalValue)
                .Tap(_ => called = true);

            Assert.True(result.Success);
            Assert.True(called);
            Assert.Equal(originalValue.ToString(), result.Value);
        }

        [Fact]
        public void Tap_Action_Is_Not_Called_On_Failure()
        {
            bool called = false;

            Result<string> result = IntToStringFailure(1)
                .Tap(_ => called = true);

            Assert.False(result.Success);
            Assert.False(called);
        }

        [Fact]
        public async Task Tap_Async_Action_Is_Called_On_Success()
        {
            int originalValue = 1;
            bool called = false;

            Result<string> result = await IntToStringAsync(originalValue)
                .Tap(async _ => { called = true; await Task.Yield(); });

            Assert.True(result.Success);
            Assert.True(called);
            Assert.Equal(originalValue.ToString(), result.Value);
        }

        [Fact]
        public async Task Tap_Async_Action_Is_Not_Called_On_Failure()
        {
            bool called = false;

            Result<string> result = await IntToStringAsyncFailure(1)
                .Tap(async _ => { called = true; await Task.Yield(); });

            Assert.False(result.Success);
            Assert.False(called);
        }

        [Fact]
        public async Task Tap_Action_On_Task_Result_Is_Called_On_Success()
        {
            int originalValue = 1;
            bool called = false;

            Result<string> result = await IntToStringAsync(originalValue)
                .Tap(_ => called = true);

            Assert.True(result.Success);
            Assert.True(called);
            Assert.Equal(originalValue.ToString(), result.Value);
        }

        [Fact]
        public async Task Tap_Async_Action_On_Task_Result_Is_Called_On_Success()
        {
            int originalValue = 1;
            bool called = false;

            Result<string> result = await IntToStringAsync(originalValue)
                .Tap(async _ => { called = true; await Task.Yield(); });

            Assert.True(result.Success);
            Assert.True(called);
            Assert.Equal(originalValue.ToString(), result.Value);
        }
    }
}
