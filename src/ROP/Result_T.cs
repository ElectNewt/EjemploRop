using System;
using System.Collections.Immutable;
using System.Net;

namespace ROP
{
    public struct Result<T>
    {
        public readonly T Value;

        public static implicit operator Result<T>(T value) => new Result<T>(value, HttpStatusCode.Accepted);

        public static implicit operator Result<T>(ImmutableArray<Error> errors) => new Result<T>(errors, HttpStatusCode.BadRequest);

        public readonly ImmutableArray<Error> Errors;
        public readonly HttpStatusCode HttpStatusCode;
        public bool Success => Errors.Length == 0;
        
        public Result(T value, HttpStatusCode statusCode)
        {
            Value = value;
            Errors = ImmutableArray<Error>.Empty;
            HttpStatusCode = statusCode;
        }
       
        public Result(ImmutableArray<Error> errors, HttpStatusCode statusCode)
        {
            if (errors.Length == 0)
            {
                throw new InvalidOperationException("You should specify at least one error");
            }

            HttpStatusCode = statusCode;
            Value = default(T);
            Errors = errors;
        }
    }
}
