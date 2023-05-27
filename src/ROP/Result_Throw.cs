using System.Collections.Immutable;
using System.Threading.Tasks;

namespace ROP
{
    public static class Result_Throw
    {
        /// <summary>
        /// Converts Result T  into T, but if there is any exception, it throws it
        /// </summary>
        /// <param name="r">result structure</param>
        /// <typeparam name="T">Type</typeparam>
        /// <returns>the object on T</returns>
        public static T Throw<T>(this Result<T> r)
        {
            if (!r.Success)
            {
                Throw(r.Errors);
            }

            return r.Value;
        }
        
        /// <summary>
        /// Converts the errors in the Array on the content of the exception
        /// </summary>
        public static void Throw(this ImmutableArray<Error> errors)
        {
            if (errors.Length > 0)
            {
                throw new ErrorResultException(errors);
            }
        }

        /// <summary>
        /// Converts Result T into T, but if there is any exception, it throws it (async)
        /// </summary>
        /// <param name="result">result structure</param>
        /// <typeparam name="T">Type</typeparam>
        /// <returns>the object on T</returns>
        public static async Task<T> ThrowAsync<T>(this Task<Result<T>> result)
        {
            return (await result).Throw();
        }
    }
}
