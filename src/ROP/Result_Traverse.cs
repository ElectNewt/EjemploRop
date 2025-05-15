using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace ROP
{
    /// <summary>
    /// Provides extension methods for traversing collections of results.
    /// </summary>
    public static class Result_Traverse
    {

        /// <summary>
        /// Converts a IEnumerable result T into a result list T
        /// </summary>
        /// <returns>A Result containing a list of T if all results are successful; otherwise, a failure result.</returns>
        public static Result<List<T>> Traverse<T>(this IEnumerable<Result<T>> results)
        {
            try
            {
                List<Error> errors = new List<Error>();
                List<T> output = new List<T>();
                HttpStatusCode firstStatusCode = HttpStatusCode.BadRequest;

                foreach (var r in results)
                {
                    if (r.Success)
                    {
                        output.Add(r.Value);
                    }
                    else
                    {
                        if (errors.Count == 0) firstStatusCode = r.HttpStatusCode;

                        errors.AddRange(r.Errors);
                    }
                }

                return errors.Count > 0
                    ? Result.Failure<List<T>>(errors.ToImmutableArray(), firstStatusCode)
                    : Result.Success(output);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        /// <summary>
        /// Converts a IEnumerable result T into a Result list T
        /// </summary>
        /// <returns>A Result containing a list of T if all results are successful; otherwise, a failure result.</returns>
        public static async Task<Result<List<T>>> Traverse<T>(this IEnumerable<Task<Result<T>>> results)
        {
            try
            {
                return (await Task.WhenAll(results)).Traverse();
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }
    }
}