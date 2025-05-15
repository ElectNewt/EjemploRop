using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace ROP
{
    /// <summary>
    /// Provides extension methods for mapping results from one type to another.
    /// </summary>
    public static class Result_Map
    {
        /// <summary>
        /// Allows to get map from a result T to U, the mapper method do not need to return a result T
        /// </summary>
        /// <returns>A result of type U.</returns>
        public static Result<U> Map<T, U>(this Result<T> r, Func<T, U> mapper)
        {
            try
            {
                return r.Bind(x => mapper(x).Success());
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        /// <summary>
        /// Allows to get map from a result T to U, the mapper method do not need to return a result T
        /// </summary>
        /// <returns>A result of type U.</returns>
        public static async Task<Result<U>> Map<T, U>(this Result<T> r, Func<T, Task<U>> mapper)
        {
            try
            {
                return await r.Bind(async x => (await mapper(x)).Success());
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        /// <summary>
        /// Allows to get map from a result T to U, the mapper method do not need to return a result T
        /// </summary>
        /// <returns>A result of type U.</returns>
        public static async Task<Result<U>> Map<T, U>(this Task<Result<T>> result, Func<T, Task<U>> mapper)
        {
            try
            {
                var r = await result;
                return await r.Map(mapper);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        /// <summary>
        /// Allows to get map from a result T to U, the mapper method do not need to return a result T
        /// </summary>
        /// <returns>A result of type U.</returns>
        public static async Task<Result<U>> Map<T, U>(this Task<Result<T>> result, Func<T, U> mapper)
        {
            try
            {
                var r = await result;
                return r.Map(mapper);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }
    }
}