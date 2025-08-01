using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using System;

namespace ROP
{
    /// <summary>
    /// Provides extension methods for executing side-effect actions in a result chain without modifying the result value.
    /// The Tap methods allow you to perform actions (such as logging, auditing, or other side effects) on successful results
    /// while preserving the original result in the chain. These methods are useful for injecting behavior without affecting the data flow.
    /// </summary>
    public static class Result_Tap
    {
        /// <summary>
        /// Executes the specified action if the result is successful, preserving the original result.
        /// Use this to perform side effects (e.g., logging) without modifying the result value.
        /// </summary>
        /// <typeparam name="T">The type of the value contained in the result.</typeparam>
        /// <param name="r">The result to tap into.</param>
        /// <param name="method">The action to execute if the result is successful.</param>
        /// <returns>The original result if successful; otherwise, a failure result.</returns>
        public static Result<T> Tap<T>(this Result<T> r, Action<T> method)
        {
            try
            {
                return r.Then(x =>
                {
                    method(x);
                    return Result.Success();
                });
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        /// <summary>
        /// Executes the specified asynchronous action if the result is successful, preserving the original result.
        /// Use this to perform asynchronous side effects (e.g., logging) without modifying the result value.
        /// </summary>
        /// <typeparam name="T">The type of the value contained in the result.</typeparam>
        /// <param name="r">The result to tap into.</param>
        /// <param name="method">The asynchronous action to execute if the result is successful.</param>
        /// <returns>A task representing the asynchronous operation, containing the original result if successful; otherwise, a failure result.</returns>
        public static async Task<Result<T>> Tap<T>(this Result<T> r, Func<T, Task> method)
        {
            try
            {
                return await r.Then(async x =>
                {
                    await method(x);
                    return Result.Success();
                });
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        /// <summary>
        /// Executes the specified action if the asynchronous result is successful, preserving the original result.
        /// Use this to perform side effects (e.g., logging) in asynchronous result chains without modifying the result value.
        /// </summary>
        /// <typeparam name="T">The type of the value contained in the result.</typeparam>
        /// <param name="result">The asynchronous result to tap into.</param>
        /// <param name="method">The action to execute if the result is successful.</param>
        /// <returns>A task representing the asynchronous operation, containing the original result if successful; otherwise, a failure result.</returns>
        public static async Task<Result<T>> Tap<T>(this Task<Result<T>> result, Action<T> method)
        {
            try
            {
                var r = await result;
                return r.Tap(x => method(x));
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        /// <summary>
        /// Executes the specified asynchronous action if the asynchronous result is successful, preserving the original result.
        /// Use this to perform asynchronous side effects (e.g., logging) in asynchronous result chains without modifying the result value.
        /// </summary>
        /// <typeparam name="T">The type of the value contained in the result.</typeparam>
        /// <param name="result">The asynchronous result to tap into.</param>
        /// <param name="method">The asynchronous action to execute if the result is successful.</param>
        /// <returns>A task representing the asynchronous operation, containing the original result if successful; otherwise, a failure result.</returns>
        public static async Task<Result<T>> Tap<T>(this Task<Result<T>> result, Func<T, Task> method)
        {
            try
            {
                var r = await result;
                return await r.Tap(x => method(x));
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }
    }
}