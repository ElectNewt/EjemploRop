using System;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace ROP
{
    public static class Result_Combine
    {
        public static Result<Unit> Combine(this Result<Unit> r1, Result<Unit> r2)
        {
            try
            {
                if (r1.Success && r2.Success)
                    return Result.Success();

                if (r1.Success)
                    return r2;

                if (r2.Success)
                    return r1;

                return Result.Failure(r1.Errors.Concat(r2.Errors));
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        public static Result<(T1, T2)> Combine<T1, T2>(this Result<T1> r1, Result<T2> r2)
        {
            try
            {

                if (r1.Success && r2.Success)
                    return Result.Success((r1.Value, r2.Value));

                if (r1.Success)
                    return r2.Errors;

                if (r2.Success)
                    return r1.Errors;

                return Result.Failure<(T1, T2)>(r1.Errors.Concat(r2.Errors).ToImmutableArray());
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        public static Result<(T1, T2)> Combine<T1, T2>(this Result<T1> r1, Func<T1, Result<T2>> action)
        {
            try
            {
                if (!r1.Success)
                    return r1.Errors;

                Result<T2> r2 = action(r1.Value);

                if (r1.Success && r2.Success)
                    return Result.Success((r1.Value, r2.Value));

                if (!r2.Success)
                    return r2.Errors;

                return Result.Failure<(T1, T2)>(r1.Errors.Concat(r2.Errors).ToImmutableArray());
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }



        public static async Task<Result<(T1, T2)>> Combine<T1, T2>(this Task<Result<T1>> result, Func<T1, Task<T2>> action)
        {
            try
            {
                Result<T1> r1 = await result;
                if (!r1.Success)
                    return r1.Errors;

                Result<T2> r2 = await action(r1.Value);

                if (r1.Success && r2.Success)
                    return Result.Success((r1.Value, r2.Value));

                if (!r2.Success)
                    return r2.Errors;

                return Result.Failure<(T1, T2)>(r1.Errors.Concat(r2.Errors).ToImmutableArray());
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        public static async Task<Result<(T1, T2, T3)>> Combine<T1, T2, T3>(this Task<Result<(T1, T2)>> result, Func<(T1, T2), Task<T3>> action)
        {
            try
            {
                Result<(T1, T2)> r12 = await result;

                if (!r12.Success)
                    return r12.Errors;

                Result<T3> r3 = await action(r12.Value);

                if (r12.Success && r3.Success)
                    return Result.Success((r12.Value.Item1, r12.Value.Item2, r3.Value));

                if (!r3.Success)
                    return r3.Errors;

                return Result.Failure<(T1, T2, T3)>(r12.Errors.Concat(r3.Errors).ToImmutableArray());
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        public static async Task<Result<(T1, T2, T3, T4)>> Combine<T1, T2, T3, T4>(this Task<Result<(T1, T2, T3)>> result, Func<(T1, T2, T3), Task<T4>> action)
        {
            try
            {
                Result<(T1, T2, T3)> r123 = await result;

                if (!r123.Success)
                    return r123.Errors;

                Result<T4> r4 = await action(r123.Value);

                if (r123.Success && r4.Success)
                    return Result.Success((r123.Value.Item1, r123.Value.Item2, r123.Value.Item3, r4.Value));

                if (!r4.Success)
                    return r4.Errors;

                return Result.Failure<(T1, T2, T3, T4)>(r123.Errors.Concat(r4.Errors).ToImmutableArray());
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

    }
}
