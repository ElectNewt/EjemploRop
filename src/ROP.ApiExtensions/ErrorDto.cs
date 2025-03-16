using System;

namespace ROP.APIExtensions
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for an error, containing a message, an optional error code, and translation variables.
    /// </summary>
    public class ErrorDto
    {
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the optional error code.
        /// </summary>
        public Guid? ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the variables used for translating the error message.
        /// </summary>
        public string[] TranslationVariables { get; set; }
    }
}
