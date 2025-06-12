using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace ROP
{
    /// <summary>
    /// Provides extension methods to combine the result of two methods, the input of the combined method is the result of the first method.
    /// </summary>
    public static class Result_Combine
    {
        /// <summary>
        /// Allows to combine the result of two methods. the input of the combined method is the result of the first method.
        /// </summary>
        /// <returns>A result chain that contains a tuple with both results</returns>
        public static Result<(T1, T2)> Combine<T1, T2>(this Result<T1> r, Func<T1, Result<T2>> action)
        {
            try
            {
                return r.Bind(action)
                        .Map(x => (r.Value, x));
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        /// <summary>
        /// Allows to combine the result of two methods. the input of the combined method is the result of the first method.
        /// </summary>
        /// <returns>A result chain that contains a tuple with both results</returns>
        public static async Task<Result<(T1, T2)>> Combine<T1, T2>(this Result<T1> r, Func<T1, Task<Result<T2>>> action)
        {
            try
            {
                return await r.Bind(action)
                              .Map(x => (r.Value, x));
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        /// <summary>
        /// Allows to combine the result of two methods. the input of the combined method is the result of the first method.
        /// </summary>
        /// <returns>A result chain that contains a tuple with both results</returns>
        public static async Task<Result<(T1, T2)>> Combine<T1, T2>(this Task<Result<T1>> result, Func<T1, Task<Result<T2>>> action)
        {
            try
            {
                Result<T1> r = await result;
                return await r.Combine(action);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        /// <summary>
        /// Allows to combine the result of two methods. the input of the combined method is the result of the first method.
        /// </summary>
        /// <returns>A result chain that contains a tuple with both results</returns>
        public static async Task<Result<(T1, T2)>> Combine<T1, T2>(this Task<Result<T1>> result, Func<T1, Result<T2>> action)
        {
            try
            {
                Result<T1> r = await result;
                return r.Combine(action);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }
    }
}