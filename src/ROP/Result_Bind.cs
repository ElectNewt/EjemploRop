using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace ROP
{
    public static class Result_Bind
    {
        public static Result<U> Bind<T, U>(this Result<T> r, Func<T, Result<U>> method)
        {
            try
            {
                return r.Success
                    ? method(r.Value)
                    : Result.Failure<U>(r.Errors);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }


        public static async Task<Result<U>> Bind<T, U>(this Task<Result<T>> result, Func<T, Task<Result<U>>> method)
        {
            try
            {
                var r = await result;
                return r.Success
                    ? await method(r.Value)
                    : Result.Failure<U>(r.Errors);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }
        public static Result<T> Then<T>(this Result<T> r, Action<T> action)
        {
            try
            {
                if (r.Success)
                {
                    action(r.Value);
                }

                return r;
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        public static async Task<Result<T>> Then<T>(this Task<Result<T>> result, Action<T> action)
        {
            try
            {
                var r = await result;
                if (r.Success)
                {
                    action(r.Value);
                }

                return r;
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        public static Result<U> Map<T, U>(this Result<T> r, Func<T, U> mapper)
        {
            try
            {
                return r.Success
                    ? Result.Success(mapper(r.Value))
                    : Result.Failure<U>(r.Errors);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }
    }
}
