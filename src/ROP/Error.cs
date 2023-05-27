using System;
using System.Collections.Generic;

namespace ROP
{
    public class Error
    {
        public readonly string Message;
        public readonly Guid? ErrorCode;
        public readonly string[] TranslationVariables;

        private Error(string message, Guid? errorCode, string[] translationVariables)
        {
            Message = message;
            ErrorCode = errorCode;
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
