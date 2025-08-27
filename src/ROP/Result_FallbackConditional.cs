using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace ROP
{
    /// <summary>
    /// Provides extension methods for handling conditional fallback logic in result chains.
    /// These methods allow you to specify a predicate to determine whether the fallback logic should be executed
    /// when the result is in an error state. If the predicate is met, the fallback method is executed; otherwise,
    /// the original error result is preserved.
    /// </summary>
    public static class Result_FallbackConditional
    {
        /// <summary>
        /// The method gets executed IF the chain is in Error state and the criteria of de Predicate is met,
        /// the previous information will be lost
        /// </summary>
        /// <returns>
        /// The original successful <see cref="Result{T}"/> if it is successful; 
        /// otherwise, the result of executing <paramref name="method"/> if the condition is met; 
        /// otherwise, the original unsuccessful <see cref="Result{T}"/>.
        /// </returns>
        public static Result<T> Fallback<T>(this Result<T> r, Predicate<Result<T>> condition, Func<T, Result<T>> method)
        {
            try
            {
                return r.Fallback(x =>
                {
                    if (condition(r)) return method(x);

                    return Result.Failure<T>(r.Errors, r.HttpStatusCode);
                });
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        /// <summary>
        /// The method gets executed IF the chain is in Error state and the criteria of de Predicate is met,
        /// the previous information will be lost
        /// </summary>
        /// <returns>
        /// The original successful <see cref="Result{T}"/> if it is successful; 
        /// otherwise, the result of executing <paramref name="method"/> if the condition is met; 
        /// otherwise, the original unsuccessful <see cref="Result{T}"/>.
        /// </returns>
        public static async Task<Result<T>> Fallback<T>(this Result<T> r, Predicate<Result<T>> condition, Func<T, Task<Result<T>>> method)
        {
            try
            {
                return await r.Fallback(async x =>
                {
                    if (condition(r)) return await method(x);

                    return Result.Failure<T>(r.Errors, r.HttpStatusCode);
                });
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        /// <summary>
        /// The method gets executed IF the chain is in Error state and the criteria of de Predicate is met,
        /// the previous information will be lost
        /// </summary>
        /// <returns>
        /// The original successful <see cref="Result{T}"/> if it is successful; 
        /// otherwise, the result of executing <paramref name="method"/> if the condition is met; 
        /// otherwise, the original unsuccessful <see cref="Result{T}"/>.
        /// </returns>
        public static async Task<Result<T>> Fallback<T>(this Task<Result<T>> result, Predicate<Result<T>> condition, Func<T, Task<Result<T>>> method)
        {
            try
            {
                var r = await result;
                return await r.Fallback(condition, method);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        /// <summary>
        /// The method gets executed IF the chain is in Error state and the criteria of de Predicate is met,
        /// the previous information will be lost
        /// </summary>
        /// <returns>
        /// The original successful <see cref="Result{T}"/> if it is successful; 
        /// otherwise, the result of executing <paramref name="method"/> if the condition is met; 
        /// otherwise, the original unsuccessful <see cref="Result{T}"/>.
        /// </returns>
        public static async Task<Result<T>> Fallback<T>(this Task<Result<T>> result, Predicate<Result<T>> condition, Func<T, Result<T>> method)
        {
            try
            {
                var r = await result;
                return r.Fallback(condition, method);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }
    }
}
