using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Net;

namespace ROP.UnitTest
{
    public class TestSuccessHTTPStatusCode : BaseResultTest
    {
        [Fact]
        public async Task TestSuccessHTTPStatusCode_SetSTatusCode_thenHttpStatuscodeIsUpdated()
        {
            Result<int> result  = await 1.Success()
                .Async()
                .UseSuccessHttpStatusCode(HttpStatusCode.OK);

            Assert.True(result.Success);
            Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
        }

        [Fact]
        public async Task TestSuccessHTTPStatusCode_thenDefaultHttpStatuscodeIsUpdated()
        {
            Result<int> result = await 1.Success()
                .Async();

            Assert.True(result.Success);
            Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
        }

        //TODO: choose which version do i want to use, only the "useSuccessStatusCode" at the end of the chain or at any point.
        //use .UseSuccessHttpStatusCode(r.HttpStatusCode) in the chain to st the value.
        [Fact]
        public void TestSuccessHTTPStatusCode_thenDefaultHttpStatuscodeIsUpdated_thenMoreChain()
        {
            Result<string> result = 1.Success()
                .UseSuccessHttpStatusCode(HttpStatusCode.OK)
                .Bind(IntToString);

            Assert.True(result.Success);
            Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
        }



    }
}
