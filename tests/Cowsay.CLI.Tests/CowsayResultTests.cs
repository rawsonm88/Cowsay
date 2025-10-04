using Cowsay.CLI.Application;
using Shouldly;
using Xunit;

namespace Cowsay.CLI.Tests;

public class CowsayResultTests
{
    [Fact]
    public void Success_CreatesResultWithExitCode0()
    {
        var result = CowsayResult.Success("Test output");

        result.ExitCode.ShouldBe(0);
        result.Output.ShouldBe("Test output");
        result.Error.ShouldBeNull();
    }

    [Fact]
    public void Success_WithEmptyOutput_CreatesResult()
    {
        var result = CowsayResult.Success(string.Empty);

        result.ExitCode.ShouldBe(0);
        result.Output.ShouldBe(string.Empty);
        result.Error.ShouldBeNull();
    }

    [Fact]
    public void Failure_CreatesResultWithExitCode1()
    {
        var result = CowsayResult.Failure("Error message");

        result.ExitCode.ShouldBe(1);
        result.Error.ShouldBe("Error message");
        result.Output.ShouldBeNull();
    }

    [Fact]
    public void Failure_WithCustomExitCode_CreatesResult()
    {
        var result = CowsayResult.Failure("Error message", 42);

        result.ExitCode.ShouldBe(42);
        result.Error.ShouldBe("Error message");
        result.Output.ShouldBeNull();
    }

    [Fact]
    public void Failure_WithEmptyError_CreatesResult()
    {
        var result = CowsayResult.Failure(string.Empty);

        result.ExitCode.ShouldBe(1);
        result.Error.ShouldBe(string.Empty);
        result.Output.ShouldBeNull();
    }
}
