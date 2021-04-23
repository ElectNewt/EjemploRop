using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net;

namespace ROP.UnitTest.ResultFailure
{
    public class TestResultFailure  : TestBaseResultFailure
    {
        protected override Result<Unit> GetResultWithString() => Result.Failure("Error");
        protected override Result<Unit> GetResultWithError() => Result.Failure(Error.Create("error"));
        protected override Result<Unit> GetResultWithArray() => Result.Failure(ImmutableArray.Create(Error.Create("Error")));
        protected override Result<Unit> GetResultWithIEnumerable() => Result.Failure(new List<Error>() {Error.Create("example")});
        protected override Result<int> GetTypedResultWithString() => Result.Failure<int>("Error");
        protected override Result<int> GetTypedResultWithError() => Result.Failure<int>(Error.Create("error"));
        protected override Result<int> GetTypedResultWithArray() => Result.Failure<int>(ImmutableArray.Create(Error.Create("Error")));
        protected override HttpStatusCode GetExpectedHttpStatusCode() => HttpStatusCode.BadRequest;
    }
}