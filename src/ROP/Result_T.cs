using System;
using System.Collections.Immutable;

namespace ROP
{
    public struct Result<T>
    {
        public readonly T Value;

        public static implicit operator Result<T>(T value) => new Result<T>(value);

        public static implicit operator Result<T>(ImmutableArray<Error> errors) => new Result<T>(errors);

        public readonly ImmutableArray<Error> Errors;
        public bool Success => Errors.Length == 0;
        
        public Result(T value)
        {
            Value = value;
            Errors = ImmutableArray<Error>.Empty;
        }
       
        public Result(ImmutableArray<Error> errors)
        {
            if (errors.Length == 0)
            {
                throw new InvalidOperationException("debes indicar almenos un error");
            }

            Value = default(T);
            Errors = errors;
        }
    }
}
