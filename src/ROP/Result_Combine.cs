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
        public static Result<(T1, T2)> Combine<T1, T2>(this Result<T1> r1, Func<T1, Result<T2>> action)
        {
            try
            {
                return r1.Bind(x => action(x))
                         .Map(x => (r1.Value, x));
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
        public static async Task<Result<(T1, T2)>> Combine<T1, T2>(this Result<T1> r1, Func<T1, Task<Result<T2>>> action)
        {
            try
            {
                return await r1.Bind(x => action(x))
                               .Map(x => (r1.Value, x));
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
                Result<T1> r1 = await result;
                return await r1.Combine(action);
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
                Result<T1> r1 = await result;
                return r1.Combine(action);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }
    }
}