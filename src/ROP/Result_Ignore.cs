using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace ROP
{
    public static class Result_Ignore
    {

        public static Result<Unit> Ignore<T>(this Result<T> r)
        {
            try
            {
                return r.Success
                    ? Result.Success()
                    : Result.Failure(r.Errors);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        public static async Task<Result<Unit>> Ignore<T>(this Task<Result<T>> r)
        {
            try
            {
                return (await r).Ignore();
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }
    }
}
