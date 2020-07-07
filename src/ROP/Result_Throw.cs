using System.Collections.Immutable;
using System.Threading.Tasks;

namespace ROP
{
    public static class Result_Throw
    {
        /// <summary>
        /// Convierte the Resut<T> a el valor de T pero si hay errores saltan las excepciones
        /// </summary>
        public static T Throw<T>(this Result<T> r)
        {
            if (!r.Success)
            {
                Throw(r.Errors);
            }

            return r.Value;
        }
        
        public static void Throw(this ImmutableArray<Error> errors)
        {
            if (errors.Length > 0)
            {
                throw new ErrorResultException(errors);
            }
        }

        /// <summary>
        /// Convierte the Resut<T> a el valor de T pero si hay errores saltan las excepciones
        /// </summary>
        public static async Task<T> ThrowAsync<T>(this Task<Result<T>> result)
        {
            return (await result).Throw();
        }
      
        public static Task ThrowAsync(this Task<Result<Unit>> result)
        {
            return result.ThrowAsync<Unit>();
        }
    }
}
