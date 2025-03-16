namespace ROP.APIExtensions
{
    internal static class ErrorDtoExtensions
    {
        internal static ErrorDto ToErrorDto(this Error error)
            => new ErrorDto()
            {
                ErrorCode = error.ErrorCode,
                Message = error.Message,
                TranslationVariables = error.TranslationVariables
            };
    }
}