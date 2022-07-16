using System;
using System.Collections.Generic;

namespace ROP
{
    public class Error
    {
        public readonly string Message;
        public readonly Guid? ErrorCode;

        private Error(string message, Guid? errorCode)
        {
            Message = message;
            ErrorCode = errorCode;
        }

        public static Error Create(string message, Guid? errorCode = null)
        {
            return new Error(message, errorCode);
        }

        public static Error Create(Guid errorCode)
        {
            return Error.Create(string.Empty, errorCode);
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
