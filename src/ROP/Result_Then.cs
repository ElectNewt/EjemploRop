using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace ROP
{
    /// <summary>
    /// Provides extension methods for executing additional methods in a result chain while ignoring their results.
    /// </summary>
    public static class Result_Then
    {
        /// <summary>
        /// Allows to execute a method on the chain, but the result is the output of the caller
        /// (its result gets ignored) Example:
        /// method 1-> returns int
        /// thenMethod returns string
        /// value on the chain -> int
        /// </summary>
        /// <returns>The original result if successful; otherwise, a failure result.</returns>
        public static Result<T> Then<T, U>(this Result<T> r, Func<T, Result<U>> method)
        {
            try
            {
                return r.Bind(x => method(x))
                        .Map(x => r.Value);
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
        /// <returns>The original result if successful; otherwise, a failure result.</returns>
        public static async Task<Result<T>> Then<T, U>(this Result<T> r, Func<T, Task<Result<U>>> method)
        {
            try
            {
                return await r.Bind(x => method(x))
                              .Map(x => r.Value);
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
        /// <returns>The original result if successful; otherwise, a failure result.</returns>
        public static async Task<Result<T>> Then<T, U>(this Task<Result<T>> result, Func<T, Task<Result<U>>> method)
        {
            try
            {
                var r = await result;
                return await r.Then(method);
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
        /// <returns>The original result if successful; otherwise, a failure result.</returns>
        public static async Task<Result<T>> Then<T, U>(this Task<Result<T>> result, Func<T, Result<U>> method)
        {
            try
            {
                var r = await result;
                return r.Then(method);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }
    }
}