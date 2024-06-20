using System;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace ROP
{
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
                if (!r1.Success)
                    return Result.Failure<(T1, T2)>(r1.Errors, r1.HttpStatusCode);

                Result<T2> r2 = action(r1.Value);

                if (r1.Success && r2.Success)
                    return Result.Success((r1.Value, r2.Value));

                if (!r2.Success)
                    return Result.Failure<(T1, T2)>(r2.Errors, r2.HttpStatusCode);

                return Result.Failure<(T1, T2)>(r1.Errors.Concat(r2.Errors).ToImmutableArray(), r1.HttpStatusCode);
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
                if (!r1.Success)
                    return Result.Failure<(T1, T2)>(r1.Errors, r1.HttpStatusCode);

                Result<T2> r2 = await action(r1.Value);

                if (r1.Success && r2.Success)
                    return Result.Success((r1.Value, r2.Value));

                if (!r2.Success)
                    Result.Failure<(T1, T2)>(r2.Errors, r2.HttpStatusCode);

                return Result.Failure<(T1, T2)>(r1.Errors.Concat(r2.Errors).ToImmutableArray(), r1.HttpStatusCode);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }
    }
}
