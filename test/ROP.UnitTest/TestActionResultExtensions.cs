using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ROP.APIExtensions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ROP.UnitTest
{
    public class TestActionResultExtensions 
    {
        [Fact]
        public async Task TestBindAllCorrect()
        {

            int originalValue = 1234;
            IActionResult apiResult = await originalValue.Success().Async().ToActionResult();

            ObjectResult result = apiResult as ObjectResult;
            ResultDto<int> resultVaue = result.Value as ResultDto<int>;

            Assert.Equal(originalValue, resultVaue.Value);
            Assert.Equal((int)HttpStatusCode.Accepted, result.StatusCode);
        }
    }
}
