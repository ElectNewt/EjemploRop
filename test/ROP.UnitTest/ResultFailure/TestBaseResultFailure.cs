using System.Net;
using Xunit;

namespace ROP.UnitTest.ResultFailure
{
    public abstract class TestBaseResultFailure
    {
        [Fact]
        public void TestResultOnStringError_ThenStatusCode()
        {
            Result<Unit> result = GetResultWithString();
            Assert.Equal(GetExpectedHttpStatusCode(), result.HttpStatusCode);
        }

        [Fact]
        public void TestResultOnError_ThenStatusCode()
        {
            Result<Unit> result = GetResultWithError();
            Assert.Equal(GetExpectedHttpStatusCode(), result.HttpStatusCode);
        }

        [Fact]
        public void TestResultOnArray_ThenStatusCode()
        {
            Result<Unit> result = GetResultWithArray();
            Assert.Equal(GetExpectedHttpStatusCode(), result.HttpStatusCode);
        }

        [Fact]
        public void TestResultOnIEnumerable_ThenStatusCode()
        {
            Result<Unit> result = GetResultWithIEnumerable();
            Assert.Equal(GetExpectedHttpStatusCode(), result.HttpStatusCode);
        }

        [Fact]
        public void TestResultTypedOnStringError_ThenStatusCode()
        {
            Result<int> result = GetTypedResultWithString();
            Assert.Equal(GetExpectedHttpStatusCode(), result.HttpStatusCode);
        }

        [Fact]
        public void TestResultTypedOnError_ThenStatusCode()
        {
            Result<int> result = GetTypedResultWithError();
            Assert.Equal(GetExpectedHttpStatusCode(), result.HttpStatusCode);
        }

        [Fact]
        public void TestResultTypedOnArray_ThenStatusCode()
        {
            Result<int> result = GetTypedResultWithArray();
            Assert.Equal(GetExpectedHttpStatusCode(), result.HttpStatusCode);
        }

        protected abstract Result<Unit> GetResultWithString();
        protected abstract Result<Unit> GetResultWithError();
        protected abstract Result<Unit> GetResultWithArray();
        protected abstract Result<Unit> GetResultWithIEnumerable();
        protected abstract Result<int> GetTypedResultWithString();
        protected abstract Result<int> GetTypedResultWithError();
        protected abstract Result<int> GetTypedResultWithArray();
        protected abstract HttpStatusCode GetExpectedHttpStatusCode();
    }
}