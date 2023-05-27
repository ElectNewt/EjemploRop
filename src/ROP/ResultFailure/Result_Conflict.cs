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
        /// Converts the type into the error flow with  HttpStatusCode.Conflict
        /// </summary>
        public static Result<T> Conflict<T>(ImmutableArray<Error> errors) => new Result<T>(errors, HttpStatusCode.Conflict);

        /// <summary>
        /// Converts the type into the error flow with  HttpStatusCode.Conflict
        /// </summary>
        public static Result<T> Conflict<T>(Error error) => new Result<T>(ImmutableArray.Create(error), HttpStatusCode.Conflict);

        /// <summary>
        /// Converts the type into the error flow with  HttpStatusCode.Conflict
        /// </summary>
        public static Result<T> Conflict<T>(string error) => new Result<T>(ImmutableArray.Create(Error.Create(error)), HttpStatusCode.Conflict);
        
        /// <summary>
        /// Converts the type into the error flow with  HttpStatusCode.Conflict
        /// </summary>
        public static Result<T> Conflict<T>(Guid errorCode) => Conflict<T>(Error.Create(errorCode));

        /// <summary>
        /// Converts the type into the error flow with  HttpStatusCode.Conflict
        /// </summary>
        public static Result<Unit> Conflict(ImmutableArray<Error> errors) => new Result<Unit>(errors, HttpStatusCode.Conflict);

        /// <summary>
        /// Converts the type into the error flow with  HttpStatusCode.Conflict
        /// </summary>
        public static Result<Unit> Conflict(IEnumerable<Error> errors) => new Result<Unit>(ImmutableArray.Create(errors.ToArray()), HttpStatusCode.Conflict);

        /// <summary>
        /// Converts the type into the error flow with  HttpStatusCode.Conflict
        /// </summary>
        public static Result<Unit> Conflict(Error error) => new Result<Unit>(ImmutableArray.Create(error), HttpStatusCode.Conflict);

        /// <summary>
        /// Converts the type into the error flow with  HttpStatusCode.Conflict
        /// </summary>
        public static Result<Unit> Conflict(string error) => new Result<Unit>(ImmutableArray.Create(Error.Create(error)), HttpStatusCode.Conflict);
        
        /// <summary>
        /// Converts the type into the error flow with  HttpStatusCode.Conflict
        /// </summary>
        public static Result<Unit> Conflict(Guid errorCode, string[] translationVariables = null) => Conflict<Unit>(Error.Create(errorCode, translationVariables));
    }
}
