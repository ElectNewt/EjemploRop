using System;
using System.Collections.Generic;

namespace ROP
{
    public class Error
    {
        public readonly string Message;
        public readonly int? ErrorCode;

        private Error(string message, int? errorCode)
        {
            Message = message;
            ErrorCode = errorCode;
        }

        public static Error Create(string message, int? errorCode = null)
        {
            return new Error(message, errorCode);
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
