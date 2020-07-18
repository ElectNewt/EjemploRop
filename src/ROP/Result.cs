using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace ROP
{
    public static class Result
    {
        public static readonly Unit Unit = Unit.Value;
        private static readonly Task<Result<Unit>> _completedUnitAsync = Task.FromResult(Success());

        public static Result<T> Success<T>(this T value) => new Result<T>(value);

        public static Result<T> Failure<T>(ImmutableArray<Error> errors) => new Result<T>(errors);

        public static Result<T> Failure<T>(Error error) => new Result<T>(ImmutableArray.Create(error));

        public static Result<T> Failure<T>(string error) => new Result<T>(ImmutableArray.Create(Error.Create(error)));

        public static Result<Unit> Success() => new Result<Unit>(Unit);

        public static Result<Unit> Failure(ImmutableArray<Error> errors) => new Result<Unit>(errors);

        public static Result<Unit> Failure(IEnumerable<Error> errors) => new Result<Unit>(ImmutableArray.Create(errors.ToArray()));

        public static Result<Unit> Failure(Error error) => new Result<Unit>(ImmutableArray.Create(error));

        public static Result<Unit> Failure(string error) => new Result<Unit>(ImmutableArray.Create(Error.Create(error)));

        public static Task<Result<T>> Async<T>(this Result<T> r) => Task.FromResult(r);

        public static Task<Result<Unit>> Async(this Result<Unit> r) => _completedUnitAsync;
    }
}
