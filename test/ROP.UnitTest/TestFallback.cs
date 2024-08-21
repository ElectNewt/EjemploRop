using Xunit;

namespace ROP.UnitTest
{
    public class TestFallback
    {

        [Fact]
        public void TestFallbackFAllsInsideFallback()
        {
            var result =
                MetodoOriginal(1)
                .Bind(x => 
                    MetodoQueFalla(x)
                    .Fallback(_=>MetodoQueDevuelveNumeroDeMeses(x))
                    );

            Assert.True(result.Success);
            Assert.Equal(12, result.Value);

        }


        [Fact]
        public void TestFallbackWhenMethodDoesntFail()
        {
            var result =
                MetodoOriginal(2)
                .Bind(x =>
                    MatodoQueMultiplica(x)
                    .Fallback(_ => MetodoQueDevuelveNumeroDeMeses(x))
                    );

            Assert.True(result.Success);
            Assert.Equal(4, result.Value);
        }

        private Result<int> MetodoOriginal(int i)
        {
            return i;
        }

        private Result<int> MetodoQueFalla(int i)
        {
            return Result.Failure<int>("error");
        }

        private Result<int> MatodoQueMultiplica(int i)
        {
            return i * i;
        }

        private Result<int> MetodoQueDevuelveNumeroDeMeses(int i)
        {
            return 12;
        }

    }
}
