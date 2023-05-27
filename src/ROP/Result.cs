using System.Net;
using System.Threading.Tasks;

namespace ROP
{
    public static partial class Result
    {
        /// <summary>
        /// Object to avoid using void
        /// </summary>
        public static readonly Unit Unit = Unit.Value;
        
        private static readonly Task<Result<Unit>> _completedUnitAsync = Task.FromResult(Success());

        /// <summary>
        /// chains an object into the Result Structure
        /// </summary>
        public static Result<T> Success<T>(this T value) => new Result<T>(value, HttpStatusCode.Accepted);
        
        /// <summary>
        /// chains an object into the Result Structure
        /// </summary>
        public static Result<T> Success<T>(this T value, HttpStatusCode httpStatusCode) => new Result<T>(value, httpStatusCode);
        
        /// <summary>
        /// chains an Result.Unit into the Result Structure
        /// </summary>
        public static Result<Unit> Success() => new Result<Unit>(Unit, HttpStatusCode.Accepted);
        
        /// <summary>
        /// Converts a synchronous Result structure into async
        /// </summary>
        public static Task<Result<T>> Async<T>(this Result<T> r) => Task.FromResult(r);
        
        /// <summary>
        /// Converts a synchronous Result structure into async
        /// </summary>
        public static Task<Result<Unit>> Async(this Result<Unit> r) => _completedUnitAsync;


    }
}
