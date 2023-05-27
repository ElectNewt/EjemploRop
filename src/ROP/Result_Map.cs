﻿using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace ROP
{
    public static class Result_Map
    {
        /// <summary>
        /// allows to get map from a result T to U, the mapper method do not need to return a result T
        /// </summary>
        public static Result<U> Map<T, U>(this Result<T> r, Func<T, U> mapper)
        {
            try
            {
                return r.Success
                    ? Result.Success(mapper(r.Value))
                    : Result.Failure<U>(r.Errors, r.HttpStatusCode);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }
        
        /// <summary>
        /// allows to get map from a result T to U, the mapper method do not need to return a result T
        /// </summary>
        public static async Task<Result<U>> Map<T, U>(this Task<Result<T>> result, Func<T, U> mapper)
        {
            try
            {
                var r = await result;
                return r.Success
                    ? Result.Success(mapper(r.Value))
                    : Result.Failure<U>(r.Errors, r.HttpStatusCode);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        public static async Task<Result<U>> Map<T, U>(this Task<Result<T>> result, Func<T, Task<U>> mapper)
        {
            try
            {
                var r = await result;
                return r.Success
                    ? Result.Success(await mapper(r.Value))
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
