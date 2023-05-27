using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace ROP
{
    public static class Result_Then
    {
        /// <summary>
        /// allows to execute a method on the chain, but the result is the output of the caller
        /// (its result gets ignored) Example:
        /// method 1-> returns int
        /// thenMethod returns string
        /// value on the chain -> int
        /// </summary>
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

        /// <summary>
        /// allows to execute a method on the chain, but the result is the output of the caller
        /// (its result gets ignored) Example:
        /// method 1-> returns int
        /// thenMethod returns string
        /// value on the chain -> int
        /// </summary>
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
