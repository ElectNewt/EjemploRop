using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ROP
{
    public static class Result
    {
        public static readonly Unit Unit = Unit.Value;
        
        public static Result<T> Success<T>(this T value) => new Result<T>(value);
        
        public static Result<T> Failure<T>(ImmutableArray<string> errors) => new Result<T>(errors);

        public static Result<T> Failure<T>(string error) => new Result<T>(ImmutableArray.Create(error));

        public static Result<Unit> Success() => new Result<Unit>(Unit);

        public static Result<Unit> Failure(ImmutableArray<string> errors) => new Result<Unit>(errors);

        public static Result<Unit> Failure(IEnumerable<string> errors) => new Result<Unit>(ImmutableArray.Create(errors.ToArray()));

        public static Result<Unit> Failure(string error) => new Result<Unit>(ImmutableArray.Create(error));
    }
}
