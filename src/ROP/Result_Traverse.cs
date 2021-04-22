using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace ROP
{
    public static class Result_Traverse
    {

        /// <summary>
        /// Convierte List<Result<T>> a Result<List<T>>
        /// </summary>
        public static Result<List<T>> Traverse<T>(this IEnumerable<Result<T>> results)
        {
            try
            {
                List<Error> errors = new List<Error>();
                List<T> output = new List<T>();
                HttpStatusCode fristStatusCode = HttpStatusCode.BadRequest;

                foreach (var r in results)
                {
                    if (r.Success)
                    {
                        output.Add(r.Value);
                    }
                    else
                    {
                        if (errors.Count == 0)
                            fristStatusCode = r.HttpStatusCode;
                        errors.AddRange(r.Errors);
                    }
                }

                return errors.Count > 0
                    ? Result.Failure<List<T>>(errors.ToImmutableArray(), fristStatusCode)
                    : Result.Success(output);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        /// <summary>
        /// Convierte IEnumerable<Task<Result<T>>> a Task<Result<List<T>>>
        /// </summary>
        public static async Task<Result<List<T>>> Traverse<T>(this IEnumerable<Task<Result<T>>> results)
        {
            try
            {
                List<Result<T>> res = new List<Result<T>>();
                foreach (var task in results)
                {
                    res.Add(await task);
                }

                return res.Traverse();
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }
    }
}
