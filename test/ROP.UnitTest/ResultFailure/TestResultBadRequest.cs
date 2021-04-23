using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net;

namespace ROP.UnitTest.ResultFailure
{
    public class TestResultBadRequest: TestBaseResultFailure
    {
        protected override Result<Unit> GetResultWithString() => Result.BadRequest("Error");
        protected override Result<Unit> GetResultWithError() => Result.BadRequest(Error.Create("error"));
        protected override Result<Unit> GetResultWithArray() => Result.BadRequest(ImmutableArray.Create(Error.Create("Error")));
        protected override Result<Unit> GetResultWithIEnumerable() => Result.BadRequest(new List<Error>() {Error.Create("example")});
        protected override Result<int> GetTypedResultWithString() => Result.BadRequest<int>("Error");
        protected override Result<int> GetTypedResultWithError() => Result.BadRequest<int>(Error.Create("error"));
        protected override Result<int> GetTypedResultWithArray() => Result.BadRequest<int>(ImmutableArray.Create(Error.Create("Error")));
        protected override HttpStatusCode GetExpectedHttpStatusCode() => HttpStatusCode.BadRequest;
    }
}