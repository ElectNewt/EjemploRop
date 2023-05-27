using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;

namespace ROP
{
    public static partial class Result
    {
        /// <summary>
        /// Converts the type into the error flow with  HttpStatusCode.BadRequest
        /// </summary>
        public static Result<T> Failure<T>(ImmutableArray<Error> errors) => new Result<T>(errors, HttpStatusCode.BadRequest);

        /// <summary>
        /// Converts the type into the error flow with  HttpStatusCode.BadRequest
        /// </summary>
        public static Result<T> Failure<T>(ImmutableArray<Error> errors, HttpStatusCode httpStatusCode) => new Result<T>(errors, httpStatusCode);

        /// <summary>
        /// Converts the type into the error flow with  HttpStatusCode.BadRequest
        /// </summary>
        public static Result<T> Failure<T>(Error error) => new Result<T>(ImmutableArray.Create(error), HttpStatusCode.BadRequest);

        /// <summary>
        /// Converts the type into the error flow with  HttpStatusCode.BadRequest
        /// </summary>
        public static Result<T> Failure<T>(string error) => new Result<T>(ImmutableArray.Create(Error.Create(error)), HttpStatusCode.BadRequest);

        /// <summary>
        /// Converts the type into the error flow with  HttpStatusCode.BadRequest
        /// </summary>
        public static Result<T> Failure<T>(Guid errorCode) => Failure<T>(Error.Create(errorCode));

        /// <summary>
        /// Converts the type into the error flow with  HttpStatusCode.BadRequest
        /// </summary>
        public static Result<Unit> Failure(ImmutableArray<Error> errors) => new Result<Unit>(errors, HttpStatusCode.BadRequest);

        /// <summary>
        /// Converts the type into the error flow with  HttpStatusCode.BadRequest
        /// </summary>
        public static Result<Unit> Failure(ImmutableArray<Error> errors, HttpStatusCode httpStatusCode) => new Result<Unit>(errors, httpStatusCode);

        /// <summary>
        /// Converts the type into the error flow with  HttpStatusCode.BadRequest
        /// </summary>
        public static Result<Unit> Failure(IEnumerable<Error> errors) =>
            new Result<Unit>(ImmutableArray.Create(errors.ToArray()), HttpStatusCode.BadRequest);

        /// <summary>
        /// Converts the type into the error flow with  HttpStatusCode.BadRequest
        /// </summary>
        public static Result<Unit> Failure(Error error) => new Result<Unit>(ImmutableArray.Create(error), HttpStatusCode.BadRequest);

        /// <summary>
        /// Converts the type into the error flow with  HttpStatusCode.BadRequest
        /// </summary>
        public static Result<Unit> Failure(string error) => new Result<Unit>(ImmutableArray.Create(Error.Create(error)), HttpStatusCode.BadRequest);

        /// <summary>
        /// Converts the type into the error flow with  HttpStatusCode.BadRequest
        /// </summary>
        public static Result<Unit> Failure(Guid errorCode, string[] translationVariables = null) =>
            Failure<Unit>(Error.Create(errorCode, translationVariables));
    }
}