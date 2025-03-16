using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace ROP
{
    /// <summary>
    /// Provides extension methods for ignoring the result of a chain.
    /// </summary>
    public static class Result_Ignore
    {
        /// <summary>
        /// Similar to fire and forget, the method gets executed, but the response is ignored
        /// </summary>
        /// <returns>A Unit Result indicating success or failure without a value.</returns>
        public static Result<Unit> Ignore<T>(this Result<T> r)
        {
            try
            {
                return r.Success
                    ? Result.Success()
                    : Result.Failure(r.Errors, r.HttpStatusCode);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        /// <summary>
        /// Similar to fire and forget, the method gets executed, but the response is ignored
        /// </summary>
        /// <returns>A Unit Result indicating success or failure without a value.</returns>
        public static async Task<Result<Unit>> Ignore<T>(this Task<Result<T>> r)
        {
            try
            {
                return (await r).Ignore();
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }
    }
}
