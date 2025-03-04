using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace ROP
{
    public static class Result_Then
    {
        /// <summary>
        /// Allows to execute a method on the chain, but the result is the output of the caller
        /// (its result gets ignored) Example:
        /// method 1-> returns int
        /// thenMethod returns string
        /// value on the chain -> int
        /// </summary>
        public static Result<T> Then<T, U>(this Result<T> r, Func<T, Result<U>> method)
        {
            try
            {
                if (!r.Success)
                {
                    return Result.Failure<T>(r.Errors, r.HttpStatusCode);
                }

                var thenResult = method(r.Value);

                return thenResult.Success ? r.Value 
                    : Result.Failure<T>(thenResult.Errors, thenResult.HttpStatusCode);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        /// <summary>
        /// Allows to execute a method on the chain, but the result is the output of the caller
        /// (its result gets ignored) Example:
        /// method 1-> returns int
        /// thenMethod returns string
        /// value on the chain -> int
        /// </summary>
        public static async Task<Result<T>> Then<T, U>(this Task<Result<T>> result, Func<T, Task<Result<U>>> method)
        {
            try
            {
                var r = await result;
                if (!r.Success)
                {
                    return Result.Failure<T>(r.Errors, r.HttpStatusCode);
                }

                var thenResult = await method(r.Value);

                return thenResult.Success ? r.Value 
                    : Result.Failure<T>(thenResult.Errors, thenResult.HttpStatusCode);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        /// <summary>
        /// Allows to execute a method on the chain, but the result is the output of the caller
        /// (its result gets ignored) Example:
        /// method 1-> returns int
        /// thenMethod returns string
        /// value on the chain -> int
        /// </summary>
        public static async Task<Result<T>> Then<T, U>(this Task<Result<T>> result, Func<T, Result<U>> method)
        {
            try
            {
                var r = await result;
                if (!r.Success)
                {
                    return Result.Failure<T>(r.Errors, r.HttpStatusCode);
                }

                var thenResult = method(r.Value);

                return thenResult.Success ? r.Value
                    : Result.Failure<T>(thenResult.Errors, thenResult.HttpStatusCode);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }
    }
}