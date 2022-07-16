using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;

namespace ROP
{
    public static partial class Result
    {
        public static Result<T> NotFound<T>(ImmutableArray<Error> errors) => new Result<T>(errors, HttpStatusCode.NotFound);

        public static Result<T> NotFound<T>(Error error) => new Result<T>(ImmutableArray.Create(error), HttpStatusCode.NotFound);

        public static Result<T> NotFound<T>(string error) => new Result<T>(ImmutableArray.Create(Error.Create(error)), HttpStatusCode.NotFound);
        public static Result<T> NotFound<T>(Guid errorCode) => NotFound<T>(Error.Create(errorCode));
        public static Result<Unit> NotFound(ImmutableArray<Error> errors) => new Result<Unit>(errors, HttpStatusCode.NotFound);

        public static Result<Unit> NotFound(IEnumerable<Error> errors) => new Result<Unit>(ImmutableArray.Create(errors.ToArray()), HttpStatusCode.NotFound);

        public static Result<Unit> NotFound(Error error) => new Result<Unit>(ImmutableArray.Create(error), HttpStatusCode.NotFound);

        public static Result<Unit> NotFound(string error) => new Result<Unit>(ImmutableArray.Create(Error.Create(error)), HttpStatusCode.NotFound);
        public static Result<Unit> NotFound(Guid errorCode) => NotFound<Unit>(Error.Create(errorCode));

    }
}
