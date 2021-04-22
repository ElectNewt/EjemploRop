using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace ROP.APIExtensions
{
    public static class ActionResultExtensions
    {
        public static IActionResult ToActionResult<T>(this Result<T> result)
        {
            return result.ToDto().ToHttpSTatusCode(result.HttpStatusCode);
        }

        public static async Task<IActionResult> ToActionResult<T>(this Task<Result<T>> result)
        {
            Result<T> r = await result;

            return r.ToDto().ToHttpSTatusCode(r.HttpStatusCode);
        }

        private static IActionResult ToHttpSTatusCode<T>(this T resultDto, HttpStatusCode statusCode)
        {
            return new ResultWithStatusCode<T>(resultDto, statusCode);

        }

        private static ResultDto<T> ToDto<T>(this Result<T> result)
        {
            if (result.Success)
                return new ResultDto<T>(result.Value);

            return new ResultDto<T>(result.Errors);
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
