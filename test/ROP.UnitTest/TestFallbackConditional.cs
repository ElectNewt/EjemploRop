using System.Threading.Tasks;
using Xunit;

namespace ROP.UnitTest
{
    public class TestFallbackConditional
    {
        [Fact]
        public void TestFallbackConditional_ConditionMet_ExecutesFallback()
        {
            var result = MetodoOriginal(1)
                .Bind(_ => MetodoQueFalla())
                .Fallback(r => !r.Success, _ => MetodoQueDevuelveNumeroDeMeses());

            Assert.True(result.Success);
            Assert.Equal(12, result.Value);
        }

        [Fact]
        public void TestFallbackConditional_ConditionNotMet_DoesNotExecuteFallback()
        {
            var result = MetodoOriginal(1)
                .Bind(_ => MetodoQueFalla())
                .Fallback(r => false, _ => MetodoQueDevuelveNumeroDeMeses());

            Assert.False(result.Success);
            Assert.Equal("error", result.Errors[0].Message);
        }

        [Fact]
        public void TestFallbackConditional_WhenMethodDoesntFail_ConditionIrrelevant()
        {
            var result = MetodoOriginal(2)
                .Bind(MatodoQueMultiplica)
                .Fallback(r => true, _ => MetodoQueDevuelveNumeroDeMeses());

            Assert.True(result.Success);
            Assert.Equal(4, result.Value);
        }

        [Fact]
        public async Task TestFallbackConditional_AsyncConditionMet_ExecutesFallback()
        {
            var result = await MetodoQueFalla().Async()
                .Fallback(r => !r.Success, _ => MetodoQueDevuelveNumeroDeMeses().Async())
                .Bind(MatodoQueMultiplica);

            Assert.True(result.Success);
            Assert.Equal(144, result.Value); // 12*12
        }

        [Fact]
        public void TestFallbackConditional_ErrorObjectCodeBasedCondition_FallbacksOnSpecificErrorCode()
        {
            var codeToFallback = System.Guid.NewGuid();

            var result = MetodoOriginal(1)
                .Bind(_ => MetodoQueFallaConErrorCode(codeToFallback))
                .Fallback(r => r.Errors[0].ErrorCode == codeToFallback, _ => MetodoQueDevuelveNumeroDeMeses());

            Assert.True(result.Success);
            Assert.Equal(12, result.Value);
        }

        [Fact]
        public void TestFallbackConditional_ErrorObjectCodeBasedCondition_DoesNotFallbackOnOtherErrorCode()
        {
            var codeToFallback = System.Guid.NewGuid();
            var codeToNotFallback = System.Guid.NewGuid();

            var resultNoFallback = MetodoOriginal(1)
                .Bind(_ => MetodoQueFallaConErrorCode(codeToNotFallback))
                .Fallback(r => r.Errors[0].ErrorCode == codeToFallback, _ => MetodoQueDevuelveNumeroDeMeses());

            Assert.False(resultNoFallback.Success);
            Assert.Equal("error", resultNoFallback.Errors[0].Message);
            Assert.Equal(codeToNotFallback, resultNoFallback.Errors[0].ErrorCode);
        }

        private Result<int> MetodoOriginal(int i)
        {
            return i;
        }

        private Result<int> MetodoQueFalla()
        {
            return Result.Failure<int>("error");
        }

        private Result<int> MetodoQueFallaConErrorCode(System.Guid code)
        {
            return Result.Failure<int>(Error.Create("error", code));
        }

        private Result<int> MatodoQueMultiplica(int i)
        {
            return i * i;
        }

        private Result<int> MetodoQueDevuelveNumeroDeMeses()
        {
            return 12;
        }
    }
}
