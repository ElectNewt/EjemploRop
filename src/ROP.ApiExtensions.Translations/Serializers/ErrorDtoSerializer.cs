using System;
using System.Globalization;
using ROP.APIExtensions;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using ROP.ApiExtensions.Translations.Language;
using ROP.ApiExtensions.Translations.Language.Extensions;

namespace ROP.ApiExtensions.Translations.Serializers
{
    public class ErrorDtoSerializer<TTranslationFile> : JsonConverter<ErrorDto>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ErrorDtoSerializer(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
            
        public override ErrorDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string errorMessage = null;
            string[] translationVariables = null;
            Guid errorCode = Guid.NewGuid();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }

                string propertyName = reader.GetString();

                if (propertyName == nameof(ErrorDto.ErrorCode))
                {
                    reader.Read();
                    errorCode = Guid.Parse(reader.GetString() ?? string.Empty);
                }

                if (propertyName == nameof(ErrorDto.Message))
                {
                    reader.Read();
                    errorMessage = reader.GetString();
                }

                if (propertyName == nameof(Error.TranslationVariables))
                {
                    using (var jsonDoc = JsonDocument.ParseValue(ref reader))
                    {
                        var result = jsonDoc.RootElement.GetRawText();
                        translationVariables = JsonSerializer.Deserialize<string[]>(result);
                    }
                }

            }

            //theoretically with the translation in place errormessage will never be null
            if (errorMessage == null && errorCode == null)
                throw new Exception("Either Message or the ErrorCode has to be populated into the error");

            return new ErrorDto()
            {
                ErrorCode = errorCode,
                Message = String.Format(errorMessage, translationVariables ?? new string[0])
            };
        }

        public override void Write(Utf8JsonWriter writer, ErrorDto value, JsonSerializerOptions options)
        {
            string errorMessageValue = value.Message;
            if (string.IsNullOrWhiteSpace(value.Message))
            {
                CultureInfo language = _httpContextAccessor.HttpContext.Request.Headers.GetCultureInfo();
                errorMessageValue = LocalizationUtils<TTranslationFile>.GetValue(value.ErrorCode.ToString(), language);
                
                if (value.TranslationVariables != null)
                {
                    errorMessageValue = string.Format(errorMessageValue, (string[])value.TranslationVariables);
                }
            }
            
            writer.WriteStartObject();
            writer.WriteString(nameof(Error.ErrorCode), value.ErrorCode.ToString());
            writer.WriteString(nameof(Error.Message), errorMessageValue);
            writer.WritePropertyName(nameof(Error.TranslationVariables));
            JsonSerializer.Serialize(writer, value.TranslationVariables ?? Array.Empty<string>(), options);
            writer.WriteEndObject();
        }
    }
}