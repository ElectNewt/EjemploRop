using System.Net;
using ROP.APIExtensions;
using Xunit;

namespace ROP.UnitTest
{
    public class TestApiCode
    {
        [Fact]
        public void Error_CreateWithApiCode_SetsApiCodeAndMessage()
        {
            var error = Error.Create(apiCode: 42201, message: "The credential is disabled.");

            Assert.Equal(42201, error.ApiCode);
            Assert.Equal("The credential is disabled.", error.Message);
            Assert.Null(error.ErrorCode);
            Assert.Null(error.TranslationVariables);
        }

        [Fact]
        public void Error_CreateWithGuid_DoesNotSetApiCode()
        {
            var guid = System.Guid.NewGuid();
            var error = Error.Create("error message", guid);

            Assert.Equal(guid, error.ErrorCode);
            Assert.Null(error.ApiCode);
        }

        [Fact]
        public void Error_CreateWithApiCodeAndGuid_SetsBothFields()
        {
            var guid = System.Guid.Parse("0b002212-4e6b-4561-96f7-8a06dbc65ac3");
            var error = Error.Create(42201, "The credential is disabled.", guid);

            Assert.Equal(42201, error.ApiCode);
            Assert.Equal(guid, error.ErrorCode);
            Assert.Equal("The credential is disabled.", error.Message);
        }

        [Fact]
        public void Error_CreateWithApiCodeAndTranslationVariables_SetsAllFields()
        {
            var guid = System.Guid.Parse("0b002212-4e6b-4561-96f7-8a06dbc65ac3");
            var variables = new[] { "a", "b" };
            var error = Error.Create(42201, "", guid, variables);

            Assert.Equal(42201, error.ApiCode);
            Assert.Equal(guid, error.ErrorCode);
            Assert.Equal(variables, error.TranslationVariables);
        }

        [Fact]
        public void Result_ConflictWithApiCode_ReturnsConflictAndExposesApiCode()
        {
            var result = Result.Conflict<System.Guid>(42201, "Forbidden operation");

            Assert.Equal(HttpStatusCode.Conflict, result.HttpStatusCode);
            Assert.False(result.Success);
            Assert.Single(result.Errors);
            Assert.Equal(42201, result.Errors[0].ApiCode);
            Assert.Equal("Forbidden operation", result.Errors[0].Message);
        }

        [Fact]
        public void Result_NotFoundWithApiCode_ReturnsNotFoundAndExposesApiCode()
        {
            var result = Result.NotFound<System.Guid>(40401, "Vehicle not found");

            Assert.Equal(HttpStatusCode.NotFound, result.HttpStatusCode);
            Assert.False(result.Success);
            Assert.Equal(40401, result.Errors[0].ApiCode);
            Assert.Equal("Vehicle not found", result.Errors[0].Message);
        }

        [Fact]
        public void Result_BadRequestWithApiCode_ReturnsBadRequestAndExposesApiCode()
        {
            var result = Result.BadRequest<System.Guid>(42299, "Field is required");

            Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatusCode);
            Assert.False(result.Success);
            Assert.Equal(42299, result.Errors[0].ApiCode);
            Assert.Equal("Field is required", result.Errors[0].Message);
        }

        [Fact]
        public void Result_FailureWithApiCode_ReturnsBadRequestAndExposesApiCode()
        {
            var result = Result.Failure<System.Guid>(42201, "Validation error");

            Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatusCode);
            Assert.False(result.Success);
            Assert.Equal(42201, result.Errors[0].ApiCode);
            Assert.Equal("Validation error", result.Errors[0].Message);
        }

        [Fact]
        public void Result_FailureUnitWithApiCode_ReturnsBadRequestAndExposesApiCode()
        {
            var result = Result.Failure(apiCode: 42202, message: "Unit error");

            Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatusCode);
            Assert.False(result.Success);
            Assert.Equal(42202, result.Errors[0].ApiCode);
            Assert.Equal("Unit error", result.Errors[0].Message);
        }

        [Fact]
        public void Result_ConflictUnitWithApiCode_ReturnsConflictAndExposesApiCode()
        {
            var result = Result.Conflict(apiCode: 40901, message: "Duplicate boarding");

            Assert.Equal(HttpStatusCode.Conflict, result.HttpStatusCode);
            Assert.False(result.Success);
            Assert.Equal(40901, result.Errors[0].ApiCode);
            Assert.Equal("Duplicate boarding", result.Errors[0].Message);
        }

        [Fact]
        public void Result_ConflictWithString_LeavesApiCodeNull()
        {
            var result = Result.Conflict<System.Guid>("plain message");

            Assert.Equal(HttpStatusCode.Conflict, result.HttpStatusCode);
            Assert.Null(result.Errors[0].ApiCode);
            Assert.Equal("plain message", result.Errors[0].Message);
        }

        [Fact]
        public void ErrorDto_ExposesApiCode()
        {
            var errorDto = new ROP.APIExtensions.ErrorDto
            {
                Message = "m",
                ApiCode = 42201
            };

            Assert.Equal(42201, errorDto.ApiCode);
        }

        [Fact]
        public void ErrorDto_DefaultApiCodeIsNull()
        {
            var errorDto = new ROP.APIExtensions.ErrorDto
            {
                Message = "m"
            };

            Assert.Null(errorDto.ApiCode);
        }

        [Fact]
        public void ToActionResult_PropagatesApiCodeIntoResultDto()
        {
            var result = Result.Conflict<int>(apiCode: 42201, message: "msg");
            var actionResult = result.ToActionResult();

            var objectResult = Assert.IsAssignableFrom<Microsoft.AspNetCore.Mvc.ObjectResult>(actionResult);
            Assert.Equal((int)HttpStatusCode.Conflict, objectResult.StatusCode);

            var dto = Assert.IsType<ROP.APIExtensions.ResultDto<int>>(objectResult.Value);
            Assert.Equal(42201, dto.Errors[0].ApiCode);
            Assert.Equal("msg", dto.Errors[0].Message);
        }
    }
}