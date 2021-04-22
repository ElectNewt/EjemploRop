using Xunit;
using System.Net;

namespace ROP.UnitTest.ResultFailure
{
    public class Test_ResultNotFound
    {
        [Fact]
        public void TestResultNotFound_ThenStatusCode()
        {
            Result<Unit> result = Result.NotFound("Error");
            Assert.Equal(HttpStatusCode.NotFound, result.HttpStatusCode);


        }
    }
}
