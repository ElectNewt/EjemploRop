using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace ROP
{
    public static class Result_Fallback
    {
        public static Result<T> Fallback<T>(this Result<T> r, Func<T, Result<T>> method)
        {
            try
            {
                return r.Success
                    ? r.Value
                    : method(r.Value);
                
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        public static async Task<Result<T>> Fallback<T>(this Task<Result<T>> r, Func<T, Task<Result<T>>> method)
        {
            try
            {
                var result = await r;
                return result.Success
                    ? result.Value
                    : await method(result.Value);

            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }


    }
}
