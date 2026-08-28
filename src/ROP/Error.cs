using System;
using System.Collections.Generic;

namespace ROP
{
    /// <summary>
    /// Represents an error that occurred during the execution of a Result.
    /// </summary>
    public class Error
    {
        /// <summary>
        /// The error message.
        /// </summary>
        public readonly string Message;

        /// <summary>
        /// The error code (Guid), used as a lookup key for i18n translations.
        /// Independent from <see cref="ApiCode"/>: keep using this field for translation,
        /// use <see cref="ApiCode"/> for machine-readable API discriminators.
        /// </summary>
        public readonly Guid? ErrorCode;

        /// <summary>
        /// The variables used for translating the error message.
        /// </summary>
        public readonly string[] TranslationVariables;

        /// <summary>
        /// Optional compact numeric code for machine-readable API discriminators
        /// (e.g. HTTP error code family + module + case, like 42201).
        /// Independent from <see cref="ErrorCode"/>: both can coexist on the same error,
        /// each serving a different purpose (ApiCode for the client, ErrorCode for i18n).
        /// Nullable to preserve backward compatibility with existing consumers.
        /// </summary>
        public readonly int? ApiCode;

        private Error(string message, Guid? errorCode, string[] translationVariables)
        {
            Message = message;
            ErrorCode = errorCode;
            TranslationVariables = translationVariables;
            ApiCode = null;
        }

        private Error(string message, Guid? errorCode, int? apiCode, string[] translationVariables)
        {
            Message = message;
            ErrorCode = errorCode;
            ApiCode = apiCode;
            TranslationVariables = translationVariables;
        }

        /// <summary>
        /// Creates a new error with a static message. Prefer Create override with the error code for automatic translations
        /// </summary>
        /// <param name="message">static message</param>
        /// <param name="errorCode">Guid specifying the error code</param>
        /// <param name="translationVariables">if your error message uses variables in the translation, you can specify them here</param>
        public static Error Create(string message, Guid? errorCode = null, string[] translationVariables = null)
        {
            return new Error(message, errorCode, translationVariables);
        }

        /// <summary>
        /// Creates a new error with an error code that can be used to resolve translated error messages. Prefer using this method.
        /// Check the docs for info on translations.
        /// </summary>
        /// <param name="errorCode">Guid specifying the error code</param>
        /// <param name="translationVariables">if your error message uses variables in the translation, you can specify them here</param>
        public static Error Create(Guid errorCode, string[] translationVariables = null)
        {
            return Error.Create(string.Empty, errorCode, translationVariables);
        }

        /// <summary>
        /// Creates a new error with a compact numeric ApiCode for machine-readable
        /// API discriminators. Optionally combines it with an <see cref="ErrorCode"/> (Guid)
        /// for i18n translation lookups. Both fields coexist on the same error, each serving
        /// a different purpose (ApiCode for the client, ErrorCode for i18n).
        /// </summary>
        /// <param name="apiCode">Compact numeric code (e.g. 42201) to be exposed as a discriminator in the HTTP response.</param>
        /// <param name="message">Human-readable error message. Can be left blank to rely on translation.</param>
        /// <param name="errorCode">Guid specifying the error code for i18n translations.</param>
        /// <param name="translationVariables">if your error message uses variables in the translation, you can specify them here</param>
        public static Error Create(int apiCode, string message, Guid? errorCode = null, string[] translationVariables = null)
        {
            return new Error(message, errorCode, apiCode, translationVariables);
        }

        /// <summary>
        /// Converts an exception into a collection of Error objects.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static IEnumerable<Error> Exception(Exception e)
        {
            if (e is ErrorResultException errs)
            {
                return errs.Errors;
            }

            return new[]
            {
                Create(e.ToString())
            };
        }
    }
}