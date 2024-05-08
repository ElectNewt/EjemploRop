using System.Threading.Tasks;
using Xunit;

namespace ROP.UnitTest;

public class TestAsync
{
    [Fact]
    public async Task WhenMethodReturningUnitFails_thenAsync_ReturnsError()
    {
        var result = await FailedMethod()
            .Async();

        Assert.False(result.Success);
    }

    private Result<Unit> FailedMethod()
    {
        return Result.Failure("example");
    }
}