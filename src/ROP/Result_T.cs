using System;
using System.Collections.Immutable;
using System.Net;

namespace ROP
{
    /// <summary>
    /// Represents the result of an operation, containing either a value of type T or a collection of errors.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the result.</typeparam>
    public struct Result<T>
    {
        /// <summary>
        /// Gets the value of the result if the operation was successful.
        /// </summary>
        public readonly T Value;

        /// <summary>
        /// Implicitly converts a value of type T to a successful result with an HTTP status code of OK.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static implicit operator Result<T>(T value) => new Result<T>(value, HttpStatusCode.OK);

        /// <summary>
        /// Implicitly converts a collection of errors to a failure result with an HTTP status code of BadRequest.
        /// </summary>
        /// <param name="errors">The collection of errors to convert.</param>
        public static implicit operator Result<T>(ImmutableArray<Error> errors) => new Result<T>(errors, HttpStatusCode.BadRequest);

        /// <summary>
        /// Gets the collection of errors.
        /// </summary>
        public readonly ImmutableArray<Error> Errors;

        /// <summary>
        /// Gets the HTTP status code associated with the result.
        /// </summary>
        public readonly HttpStatusCode HttpStatusCode;

        /// <summary>
        /// Gets a value indicating whether the operation was successful.
        /// </summary>
        public bool Success => Errors.Length == 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> struct with a successful value and status code.
        /// </summary>
        /// <param name="value">The value of the result.</param>
        /// <param name="statusCode">The HTTP status code associated with the result.</param>
        public Result(T value, HttpStatusCode statusCode)
        {
            Value = value;
            Errors = ImmutableArray<Error>.Empty;
            HttpStatusCode = statusCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> struct with a collection of errors and status code.
        /// </summary>
        /// <param name="errors">The collection of errors.</param>
        /// <param name="statusCode">The HTTP status code associated with the result.</param>
        /// <exception cref="InvalidOperationException">Thrown when the errors collection is empty.</exception>
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
