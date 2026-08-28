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
        /// Gets or sets the optional error code (Guid), used as a lookup key for i18n translations.
        /// </summary>
        public Guid? ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the optional compact numeric API discriminator (e.g. 42201).
        /// Independent from <see cref="ErrorCode"/>: each serves a different purpose
        /// (ApiCode for machine-readable client discriminators, ErrorCode for i18n).
        /// Will be serialized only when populated.
        /// </summary>
        public int? ApiCode { get; set; }

        /// <summary>
        /// Gets or sets the variables used for translating the error message.
        /// </summary>
        public string[] TranslationVariables { get; set; }
    }
}
