using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace ROP
{
    public static class Result_Then
    {
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
    }
}
