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
        public static Result<T> BadRequest<T>(ImmutableArray<Error> errors) => new Result<T>(errors, HttpStatusCode.BadRequest);

        /// <summary>
        /// Converts the type into the error flow with  HttpStatusCode.BadRequest
        /// </summary>
        public static Result<T> BadRequest<T>(Error error) => new Result<T>(ImmutableArray.Create(error), HttpStatusCode.BadRequest);

        /// <summary>
        /// Converts the type into the error flow with  HttpStatusCode.BadRequest
        /// </summary>
        public static Result<T> BadRequest<T>(string error) => new Result<T>(ImmutableArray.Create(Error.Create(error)), HttpStatusCode.BadRequest);

        /// <summary>
        /// Converts the type into the error flow with  HttpStatusCode.BadRequest
        /// </summary>
        public static Result<T> BadRequest<T>(Guid errorCode) => BadRequest<T>(Error.Create(errorCode));

        /// <summary>
        /// Converts the type into the error flow with HttpStatusCode.BadRequest using a compact
        /// numeric <see cref="Error.ApiCode"/> as discriminator. Useful for APIs that need a
        /// machine-readable code (e.g. 42201) in the HTTP response instead of a Guid.
        /// </summary>
        /// <param name="apiCode">Compact numeric code exposed as discriminator in the HTTP response.</param>
        /// <param name="message">Human-readable error message.</param>
        public static Result<T> BadRequest<T>(int apiCode, string message) => new Result<T>(ImmutableArray.Create(Error.Create(apiCode, message)), HttpStatusCode.BadRequest);

        /// <summary>
        /// Converts the type into the error flow with  HttpStatusCode.BadRequest
        /// </summary>
        public static Result<Unit> BadRequest(ImmutableArray<Error> errors) => new Result<Unit>(errors, HttpStatusCode.BadRequest);

        /// <summary>
        /// Converts the type into the error flow with  HttpStatusCode.BadRequest
        /// </summary>
        public static Result<Unit> BadRequest(IEnumerable<Error> errors) =>
            new Result<Unit>(ImmutableArray.Create(errors.ToArray()), HttpStatusCode.BadRequest);

        /// <summary>
        /// Converts the type into the error flow with  HttpStatusCode.BadRequest
        /// </summary>
        public static Result<Unit> BadRequest(Error error) => new Result<Unit>(ImmutableArray.Create(error), HttpStatusCode.BadRequest);

        /// <summary>
        /// Converts the type into the error flow with  HttpStatusCode.BadRequest
        /// </summary>
        public static Result<Unit> BadRequest(string error) =>
            new Result<Unit>(ImmutableArray.Create(Error.Create(error)), HttpStatusCode.BadRequest);

        /// <summary>
        /// Converts the type into the error flow with  HttpStatusCode.BadRequest
        /// </summary>
        public static Result<Unit> BadRequest(Guid errorCode, string[] translationVariables = null) =>
            BadRequest<Unit>(Error.Create(errorCode, translationVariables));

        /// <summary>
        /// Converts the type into the error flow with HttpStatusCode.BadRequest using a compact
        /// numeric <see cref="Error.ApiCode"/> as discriminator. Useful for APIs that need a
        /// machine-readable code (e.g. 42201) in the HTTP response instead of a Guid.
        /// </summary>
        /// <param name="apiCode">Compact numeric code exposed as discriminator in the HTTP response.</param>
        /// <param name="message">Human-readable error message.</param>
        public static Result<Unit> BadRequest(int apiCode, string message) => BadRequest<Unit>(Error.Create(apiCode, message));
    }
}