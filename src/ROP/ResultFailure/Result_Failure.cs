using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;

namespace ROP
{
    public static partial class Result
    {

        public static Result<T> Failure<T>(ImmutableArray<Error> errors) => new Result<T>(errors, HttpStatusCode.BadRequest);
        public static Result<T> Failure<T>(ImmutableArray<Error> errors, HttpStatusCode httpStatusCode) => new Result<T>(errors, httpStatusCode);

        public static Result<T> Failure<T>(Error error) => new Result<T>(ImmutableArray.Create(error), HttpStatusCode.BadRequest);

        public static Result<T> Failure<T>(string error) => new Result<T>(ImmutableArray.Create(Error.Create(error)), HttpStatusCode.BadRequest);

        public static Result<Unit> Failure(ImmutableArray<Error> errors) => new Result<Unit>(errors, HttpStatusCode.BadRequest);
        public static Result<Unit> Failure(ImmutableArray<Error> errors, HttpStatusCode httpStatusCode) => new Result<Unit>(errors, httpStatusCode);

        public static Result<Unit> Failure(IEnumerable<Error> errors) => new Result<Unit>(ImmutableArray.Create(errors.ToArray()), HttpStatusCode.BadRequest);

        public static Result<Unit> Failure(Error error) => new Result<Unit>(ImmutableArray.Create(error), HttpStatusCode.BadRequest);

        public static Result<Unit> Failure(string error) => new Result<Unit>(ImmutableArray.Create(Error.Create(error)), HttpStatusCode.BadRequest);

    }
}
