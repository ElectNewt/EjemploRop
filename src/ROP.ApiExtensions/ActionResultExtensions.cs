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
        /// </summary>
        public static IActionResult ToActionResult<T>(this Result<T> result)
        {
            return result.ToDto().ToHttpStatusCode(result.HttpStatusCode);
        }

        /// <summary>
        /// Converts a result T chain, into an IActionResult, it uses the HttpStatusCode on the result chain
        /// use .UseSuccessHttpStatusCode(HttpStatusCode) if you want to set it up.
        /// </summary>
        public static async Task<IActionResult> ToActionResult<T>(this Task<Result<T>> result)
        {
            Result<T> r = await result;

            return r.ToActionResult();
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
                Errors = result.Errors.Select(x => new ErrorDto()
                {
                    ErrorCode = x.ErrorCode,
                    Message = x.Message
                }).ToImmutableArray()
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
