using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace ROP
{
    /// <summary>
    /// Provides extension methods for executing side-effect actions in a result chain when the result is an error, without modifying the result value.
    /// The TapError methods allow you to perform actions (such as logging, auditing, or other side effects) on error results
    /// while preserving the original result in the chain. These methods are useful for injecting behavior on the error path without affecting the data flow.
    /// </summary>
    public static class Result_TapError
    {
        /// <summary>
        /// Executes the specified action if the result is a failure, preserving the original result.
        /// Use this to perform side effects (e.g., logging) when an error occurs, without modifying the result value.
        /// </summary>
        /// <typeparam name="T">The type of the value contained in the result.</typeparam>
        /// <param name="r">The result to tap into.</param>
        /// <param name="method">The action to execute if the result is a failure. Receives the collection of errors.</param>
        /// <returns>The original result.</returns>
        public static Result<T> TapError<T>(this Result<T> r, Action<T> method)
        {
            try
            {
                return r.Fallback(x =>
                {
                    method(x);
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
        /// Executes the specified asynchronous action if the result is a failure, preserving the original result.
        /// Use this to perform asynchronous side effects (e.g., logging) when an error occurs, without modifying the result value.
        /// </summary>
        /// <typeparam name="T">The type of the value contained in the result.</typeparam>
        /// <param name="r">The result to tap into.</param>
        /// <param name="method">The asynchronous action to execute if the result is a failure. Receives the collection of errors.</param>
        /// <returns>A task representing the asynchronous operation, containing the original result.</returns>
        public static async Task<Result<T>> TapError<T>(this Result<T> r, Func<T, Task> method)
        {
            try
            {
                return await r.Fallback(async x =>
                {
                    await method(x);
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
        /// Executes the specified action if the asynchronous result is a failure, preserving the original result.
        /// Use this to perform side effects (e.g., logging) in asynchronous result chains when an error occurs, without modifying the result value.
        /// </summary>
        /// <typeparam name="T">The type of the value contained in the result.</typeparam>
        /// <param name="result">The asynchronous result to tap into.</param>
        /// <param name="method">The action to execute if the result is a failure. Receives the collection of errors.</param>
        /// <returns>A task representing the asynchronous operation, containing the original result.</returns>
        public static async Task<Result<T>> TapError<T>(this Task<Result<T>> result, Action<T> method)
        {
            try
            {
                var r = await result;
                return r.TapError(method);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        /// <summary>
        /// Executes the specified asynchronous action if the asynchronous result is a failure, preserving the original result.
        /// Use this to perform asynchronous side effects (e.g., logging) in asynchronous result chains when an error occurs, without modifying the result value.
        /// </summary>
        /// <typeparam name="T">The type of the value contained in the result.</typeparam>
        /// <param name="result">The asynchronous result to tap into.</param>
        /// <param name="method">The asynchronous action to execute if the result is a failure. Receives the collection of errors.</param>
        /// <returns>A task representing the asynchronous operation, containing the original result.</returns>
        public static async Task<Result<T>> TapError<T>(this Task<Result<T>> result, Func<T, Task> method)
        {
            try
            {
                var r = await result;
                return await r.TapError(method);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }
    }
}