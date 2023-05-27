using System;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace ROP
{
    public static class Result_Bind
    {
        
        /// <summary>
        /// Allows to chain two methods, the output of the first is the input of the second.
        /// </summary>
        /// <param name="r">current Result chain</param>
        /// <param name="method">method to execute</param>
        /// <typeparam name="T">Input type</typeparam>
        /// <typeparam name="U">Output type</typeparam>
        /// <returns>Result Structure of the return type</returns>
        public static Result<U> Bind<T, U>(this Result<T> r, Func<T, Result<U>> method)
        {
            try
            {
                return r.Success
                    ? method(r.Value)
                    : Result.Failure<U>(r.Errors, r.HttpStatusCode);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }


        /// <summary>
        /// Allows to chain two async methods, the output of the first is the input of the second.
        /// </summary>
        /// <param name="result">current Result chain</param>
        /// <param name="method">method to execute</param>
        /// <typeparam name="T">Input type</typeparam>
        /// <typeparam name="U">Output type</typeparam>
        /// <returns>Async Result Structure of the return type</returns>
        public static async Task<Result<U>> Bind<T, U>(this Task<Result<T>> result, Func<T, Task<Result<U>>> method)
        {
            try
            {
                var r = await result;
                return r.Success
                    ? await method(r.Value)
                    : Result.Failure<U>(r.Errors, r.HttpStatusCode);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }


        /// <summary>
        /// Allows to chain an async method to a non async method, the output of the first is the input of the second.
        /// </summary>
        /// <param name="result">current Result chain</param>
        /// <param name="method">method to execute</param>
        /// <typeparam name="T">Input type</typeparam>
        /// <typeparam name="U">Output type</typeparam>
        /// <returns>Async Result Structure of the return type</returns>
        public static async Task<Result<U>> Bind<T, U>(this Task<Result<T>> result, Func<T, Result<U>> method)
        {
            try
            {
                var r = await result;

                return r.Success
                    ? method(r.Value)
                    : Result.Failure<U>(r.Errors, r.HttpStatusCode);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }
    }
}
