using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task WhenSuccess_andToResultOrProblemDetails_ReturnObject()
        {
            int originalValue = 1234;
            IActionResult apiResult = await originalValue.Success().Async().ToValueOrProblemDetails();

            ObjectResult result = apiResult as ObjectResult;
            int? resultVaue = result.Value as int?;
            Assert.Equal(originalValue, resultVaue);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task WhenErrorWithOnlyMessage_andToResultOrProblemDetails_ReturnProblemDetailsWithNoErrorCode()
        {
            string originalErrorValue = "error";
            IActionResult apiResult =
                await Result.BadRequest<int>(originalErrorValue).Async().ToValueOrProblemDetails();

            ObjectResult result = apiResult as ObjectResult;
            ProblemDetails? resultVaue = result.Value as ProblemDetails;
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Error(s) found", resultVaue.Title);
            Assert.Equal("One or more errors occurred", resultVaue.Detail);
            Assert.Single(resultVaue.Extensions);
            var extension = resultVaue.Extensions.First();
            Assert.Equal("Errors", extension.Key);
            var errorDtos = extension.Value as List<ErrorDto>;
            Assert.Single(errorDtos);
            Assert.Equal(originalErrorValue, errorDtos.First().Message);
            Assert.Null(errorDtos.First().ErrorCode);
        }

        [Fact]
        public async Task WhenErrorWithError_andToResultOrProblemDetails_ReturnProblemDetails()
        {
            Error originalErrorValue = Error.Create("ErrorMessage", Guid.NewGuid());
            IActionResult apiResult =
                await Result.BadRequest<int>(originalErrorValue).Async().ToValueOrProblemDetails();

            ObjectResult result = apiResult as ObjectResult;
            ProblemDetails? resultVaue = result.Value as ProblemDetails;
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Error(s) found", resultVaue.Title);
            Assert.Equal("One or more errors occurred", resultVaue.Detail);
            Assert.Single(resultVaue.Extensions);
            var extension = resultVaue.Extensions.First();
            Assert.Equal("Errors", extension.Key);
            var errorDtos = extension.Value as List<ErrorDto>;
            Assert.Single(errorDtos);
            Assert.Equal(originalErrorValue.ErrorCode, errorDtos.First().ErrorCode);
            Assert.Equal(originalErrorValue.Message, errorDtos.First().Message);
            Assert.Equal(originalErrorValue.TranslationVariables, errorDtos.First().TranslationVariables);

        }
    }
}