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
            JsonSerializerOptions serializeOptions = GetSerializerOptions();

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
          
            JsonSerializerOptions serializeOptions = GetSerializerOptions();
            
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
        
        [Fact]
        public void When_translation_contains_variables_message_gets_fromated()
        {
            JsonSerializerOptions serializeOptions = GetSerializerOptions();

            ResultDto<Unit> obj = new ResultDto<Unit>()
            {
                Value = null,
                Errors = new List<ErrorDto>()
                {
                    new ErrorDto()
                    {
                        ErrorCode = ErrorTranslations.ErrorExampleWithVariables,
                        TranslationVariables = new []{"1", "2"}
                    }
                }.ToImmutableArray()
            };

            string json = JsonSerializer.Serialize(obj, serializeOptions);
            var resultDto = JsonSerializer.Deserialize<ResultDto<Unit>>(json);
            Assert.Equal("message translated with variable of value 1 and a second one 2", resultDto.Errors.First().Message);
        }

        private JsonSerializerOptions GetSerializerOptions()
        {
            Mock<IHeaderDictionary> mockHeader = new Mock<IHeaderDictionary>();
            mockHeader.Setup(a => a["Accept-Language"]).Returns("en;q=0.4");
            Mock<IHttpContextAccessor> httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(a => a.HttpContext.Request.Headers).Returns(mockHeader.Object);

            JsonSerializerOptions serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new ErrorDtoSerializer<ErrorTranslations>(httpContextAccessorMock.Object));
            return serializeOptions;
        }
    }
}