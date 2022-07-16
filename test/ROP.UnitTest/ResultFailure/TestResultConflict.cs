using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net;
namespace ROP.UnitTest.ResultFailure
{
    public class TestResultConflict: TestBaseResultFailure
    {
        protected override Result<Unit> GetResultWithString() => Result.Conflict("Error");
        protected override Result<Unit> GetResultWithGuid() => Result.Conflict(Guid.NewGuid());
        protected override Result<Unit> GetResultWithError() => Result.Conflict(Error.Create("error"));
        protected override Result<Unit> GetResultWithArray() => Result.Conflict(ImmutableArray.Create(Error.Create("Error")));
        protected override Result<Unit> GetResultWithIEnumerable() => Result.Conflict(new List<Error>() {Error.Create("example")});
        protected override Result<int> GetTypedResultWithString() => Result.Conflict<int>("Error");
        protected override Result<int> GetTypedResultWithError() => Result.Conflict<int>(Error.Create("error"));
        protected override Result<int> GetTypedResultWithArray() => Result.Conflict<int>(ImmutableArray.Create(Error.Create("Error")));
        protected override HttpStatusCode GetExpectedHttpStatusCode() => HttpStatusCode.Conflict;
    }
}