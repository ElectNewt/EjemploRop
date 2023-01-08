using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;

namespace ROP
{
    public static partial class Result
    {
        public static Result<T> BadRequest<T>(ImmutableArray<Error> errors) => new Result<T>(errors, HttpStatusCode.BadRequest);

        public static Result<T> BadRequest<T>(Error error) => new Result<T>(ImmutableArray.Create(error), HttpStatusCode.BadRequest);

        public static Result<T> BadRequest<T>(string error) => new Result<T>(ImmutableArray.Create(Error.Create(error)), HttpStatusCode.BadRequest);
        public static Result<T> BadRequest<T>(Guid errorCode) => BadRequest<T>(Error.Create(errorCode));

        public static Result<Unit> BadRequest(ImmutableArray<Error> errors) => new Result<Unit>(errors, HttpStatusCode.BadRequest);

        public static Result<Unit> BadRequest(IEnumerable<Error> errors) => new Result<Unit>(ImmutableArray.Create(errors.ToArray()), HttpStatusCode.BadRequest);

        public static Result<Unit> BadRequest(Error error) => new Result<Unit>(ImmutableArray.Create(error), HttpStatusCode.BadRequest);

        public static Result<Unit> BadRequest(string error) => new Result<Unit>(ImmutableArray.Create(Error.Create(error)), HttpStatusCode.BadRequest);
        public static Result<Unit> BadRequest(Guid errorCode, string[] translationVariables = null) => BadRequest<Unit>(Error.Create(errorCode, translationVariables));

    }
}
