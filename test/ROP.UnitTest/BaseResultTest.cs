using System;
using System.Threading.Tasks;

namespace ROP.UnitTest
{
    public class BaseResultTest
    {
        protected Result<string> IntToString(int i)
            => i.ToString();
        protected Task<Result<string>> IntToStringAsync(int i)
            => IntToString(i).Async();
        protected Result<string> IntToStringFailure(int i)
            => Result.Failure<string>("There is an error");
        protected Task<Result<string>> IntToStringAsyncFailure(int i)
            => IntToStringFailure(i).Async();

        protected Result<int> StringIntoInt(string s)
            => Convert.ToInt32(s);
        protected Task<Result<int>> StringIntoIntAsync(string s)
            => StringIntoInt(s).Async();
        protected Result<int> StringIntoIntFailure(string s)
            => Result.NotFound<int>("There is an error");
        protected Task<Result<int>> StringIntoIntAsyncFailure(string s)
            => StringIntoIntFailure(s).Async();
    }
}
