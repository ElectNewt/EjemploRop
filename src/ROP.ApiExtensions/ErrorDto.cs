using System;

namespace ROP.APIExtensions
{
    public class ErrorDto
    {
        public string Message { get; set; }
        public Guid? ErrorCode { get; set; }
        public string[] TranslationVariables { get; set; }
    }
}
