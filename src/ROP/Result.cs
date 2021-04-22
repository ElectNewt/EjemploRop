using System.Net;
using System.Threading.Tasks;

namespace ROP
{
    public static partial class Result
    {
        public static readonly Unit Unit = Unit.Value;
        private static readonly Task<Result<Unit>> _completedUnitAsync = Task.FromResult(Success());

        public static Result<T> Success<T>(this T value) => new Result<T>(value, HttpStatusCode.Accepted);
        public static Result<T> Success<T>(this T value, HttpStatusCode httpStatusCode) => new Result<T>(value, httpStatusCode);
        public static Result<Unit> Success() => new Result<Unit>(Unit, HttpStatusCode.Accepted);
        public static Task<Result<T>> Async<T>(this Result<T> r) => Task.FromResult(r);
        public static Task<Result<Unit>> Async(this Result<Unit> r) => _completedUnitAsync;


    }
}
