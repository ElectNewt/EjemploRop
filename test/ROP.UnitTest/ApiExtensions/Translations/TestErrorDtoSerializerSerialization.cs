using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Moq;
using ROP.APIExtensions;
using ROP.ApiExtensions.Translations.Serializers;
using Xunit;

namespace ROP.UnitTest.ApiExtensions.Translations
{
    public class TestErrorDtoSerializerSerialization
    {
        [Fact]
        public void When_message_is_empty_then_translate()
        {
            Mock<IHeaderDictionary> mockHeader = new Mock<IHeaderDictionary>();
            mockHeader.Setup(a => a["Accept-Language"]).Returns("en;q=0.4");
            Mock<IHttpContextAccessor> httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(a => a.HttpContext.Request.Headers).Returns(mockHeader.Object);

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new ErrorDtoSerializer<ErrorTranslations>(httpContextAccessorMock.Object));

            ResultDto<Unit> obj = new ResultDto<Unit>()
            {
                Value = null,
                Errors = new List<ErrorDto>()
                {
                    new ErrorDto()
                    {
                        ErrorCode = ErrorTranslations.ErrorExample
                    }
                }.ToImmutableArray()
            };

            string json = JsonSerializer.Serialize(obj, serializeOptions);
            var resultDto = JsonSerializer.Deserialize<ResultDto<Unit>>(json);
            Assert.Equal("This is the message Translated", resultDto.Errors.First().Message);
        }
        
        [Fact]
        public void When_message_is_populated_translation_getsIgnored()
        {
            Mock<IHeaderDictionary> mockHeader = new Mock<IHeaderDictionary>();
            mockHeader.Setup(a => a["Accept-Language"]).Returns("en;q=0.4");
            Mock<IHttpContextAccessor> httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(a => a.HttpContext.Request.Headers).Returns(mockHeader.Object);

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new ErrorDtoSerializer<ErrorTranslations>(httpContextAccessorMock.Object));

            ResultDto<Unit> obj = new ResultDto<Unit>()
            {
                Value = null,
                Errors = new List<ErrorDto>()
                {
                    new ErrorDto()
                    {
                        Message = "example message",
                        ErrorCode = ErrorTranslations.ErrorExample
                    }
                }.ToImmutableArray()
            };

            string json = JsonSerializer.Serialize(obj, serializeOptions);
            var resultDto = JsonSerializer.Deserialize<ResultDto<Unit>>(json);
            Assert.Equal(obj.Errors.First().Message, resultDto.Errors.First().Message);
        }
    }
}