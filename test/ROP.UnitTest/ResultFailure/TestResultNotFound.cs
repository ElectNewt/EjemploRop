using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net;

namespace ROP.UnitTest.ResultFailure
{
    public class TestResultNotFound : TestBaseResultFailure
    {
        protected override Result<Unit> GetResultWithString() => Result.NotFound("Error");
        protected override Result<Unit> GetResultWithGuid() => Result.NotFound(Guid.NewGuid());
        protected override Result<Unit> GetResultWithError() => Result.NotFound(Error.Create("error"));
        protected override Result<Unit> GetResultWithArray() => Result.NotFound(ImmutableArray.Create(Error.Create("Error")));
        protected override Result<Unit> GetResultWithIEnumerable() => Result.NotFound(new List<Error>() {Error.Create("example")});
        protected override Result<int> GetTypedResultWithString() => Result.NotFound<int>("Error");
        protected override Result<int> GetTypedResultWithError() => Result.NotFound<int>(Error.Create("error"));
        protected override Result<int> GetTypedResultWithArray() => Result.NotFound<int>(ImmutableArray.Create(Error.Create("Error")));
        protected override HttpStatusCode GetExpectedHttpStatusCode() => HttpStatusCode.NotFound;
    }
}