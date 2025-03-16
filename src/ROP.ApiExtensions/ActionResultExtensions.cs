using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ROP.APIExtensions
{
    /// <summary>
    /// Provides extension methods to convert results into IActionResult.
    /// </summary>
    public static class ActionResultExtensions
    {
        /// <summary>
        /// Converts a result T chain, into an IActionResult, it uses the HttpStatusCode on the result chain
        /// use .UseSuccessHttpStatusCode(HttpStatusCode) if you want to set it up.
        /// </summary>
        /// <typeparam name="T">The type of the value in the result.</typeparam>
        /// <param name="result">The result to convert.</param>
        /// <returns>Returns a ResultDto of T. If you want to return T or ProblemDetails in case of an error, use ToResultOrProblemDetails.</returns>
        public static IActionResult ToActionResult<T>(this Result<T> result)
        {
            return result.ToDto().ToHttpStatusCode(result.HttpStatusCode);
        }

        /// <summary>
        /// Converts a result T chain, into an IActionResult, it uses the HttpStatusCode on the result chain
        /// use .UseSuccessHttpStatusCode(HttpStatusCode) if you want to set it up.
        /// </summary>
        /// <typeparam name="T">The type of the value in the result.</typeparam>
        /// <param name="result">The result to convert.</param>
        /// <returns>Returns a ResultDto of T. If you want to return T or ProblemDetails in case of an error, use ToResultOrProblemDetails.</returns>
        public static async Task<IActionResult> ToActionResult<T>(this Task<Result<T>> result)
        {
            Result<T> r = await result;

            return r.ToActionResult();
        }


        /// <summary>
        /// Converts a result T chain, into an IActionResult with the value or ProblemDetails.
        /// It uses the HttpStatusCode on the result chain. use .UseSuccessHttpStatusCode(HttpStatusCode) if you want to set it up.
        /// </summary>
        /// <returns>Returns the value or a ProblemDetails in case of an error.</returns>
        public static IActionResult ToValueOrProblemDetails<T>(this Result<T> result)
        {
            if (result.Success)
            {
                return result.Value.ToHttpStatusCode(result.HttpStatusCode);
            }

            ProblemDetails problemDetails = new ProblemDetails()
            {
                Title = "Error(s) found",
                Status = (int)result.HttpStatusCode,
                Detail = "One or more errors occurred",
            };

            problemDetails.Extensions.Add("ValidationErrors", result.Errors.Select(x => x.ToErrorDto()).ToList());

            return problemDetails.ToHttpStatusCode(result.HttpStatusCode);
        }

        /// <summary>
        /// Converts a result T chain, into an IActionResult with the value or ProblemDetails.
        /// It uses the HttpStatusCode on the result chain. use .UseSuccessHttpStatusCode(HttpStatusCode) if you want to set it up.
        /// </summary>
        /// <returns>Returns the value or a ProblemDetails in case of an error.</returns>
        public static async Task<IActionResult> ToValueOrProblemDetails<T>(this Task<Result<T>> result)
        {
            Result<T> r = await result;

            return r.ToValueOrProblemDetails();
        }

        private static IActionResult ToHttpStatusCode<T>(this T resultDto, HttpStatusCode statusCode)
        {
            return new ResultWithStatusCode<T>(resultDto, statusCode);
        }

        private static ResultDto<T> ToDto<T>(this Result<T> result)
        {
            if (result.Success)
                return new ResultDto<T>()
                {
                    Value = result.Value,
                    Errors = ImmutableArray<ErrorDto>.Empty
                };

            return new ResultDto<T>()
            {
                Value = default,
                Errors = result.Errors.Select(x => x.ToErrorDto()).ToImmutableArray()
            };
        }


        private class ResultWithStatusCode<T> : ObjectResult
        {
            public ResultWithStatusCode(T content, HttpStatusCode httpStatusCode)
                : base(content)
            {
                StatusCode = (int)httpStatusCode;
            }
        }
    }
}