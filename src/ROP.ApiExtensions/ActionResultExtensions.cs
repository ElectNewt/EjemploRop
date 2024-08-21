using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ROP.APIExtensions
{
    public static class ActionResultExtensions
    {
        /// <summary>
        /// Converts a result T chain, into an IActionResult, it uses the HttpStatusCode on the result chain
        /// use .UseSuccessHttpStatusCode(HttpStatusCode) if you want to set it up.
        /// It returns a ResultDto of T, if you want to return T or problemDetails in case of an error, use ToResultOrProblemDetails
        /// </summary>
        public static IActionResult ToActionResult<T>(this Result<T> result)
        {
            return result.ToDto().ToHttpStatusCode(result.HttpStatusCode);
        }

        /// <summary>
        /// Converts a result T chain, into an IActionResult, it uses the HttpStatusCode on the result chain
        /// use .UseSuccessHttpStatusCode(HttpStatusCode) if you want to set it up.
        /// It returns a ResultDto of T, if you want to return T or problemDetails in case of an error, use ToResultOrProblemDetails
        /// </summary>
        public static async Task<IActionResult> ToActionResult<T>(this Task<Result<T>> result)
        {
            Result<T> r = await result;

            return r.ToActionResult();
        }


        /// <summary>
        /// Converts a result T chain, into an IActionResult with the value or ProblemDetails.
        /// It uses the HttpStatusCode on the result chain. use .UseSuccessHttpStatusCode(HttpStatusCode) if you want to set it up.
        /// This method is useful when you want to return the value or a ProblemDetails in case of an error.
        /// </summary>
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
        /// This method is useful when you want to return the value or a ProblemDetails in case of an error.
        /// </summary>
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