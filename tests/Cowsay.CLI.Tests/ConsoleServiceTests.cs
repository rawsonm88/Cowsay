using System;
using Cowsay.CLI.Infrastructure;
using Shouldly;
using Xunit;

namespace Cowsay.CLI.Tests;

public class ConsoleServiceTests
{
    [Fact]
    public void Out_ReturnsConsoleOut()
    {
        var service = new ConsoleService();

        var result = service.Out;

        result.ShouldBe(Console.Out);
    }

    [Fact]
    public void Error_ReturnsConsoleError()
    {
        var service = new ConsoleService();

        var result = service.Error;

        result.ShouldBe(Console.Error);
    }

    [Fact]
    public void IsInputRedirected_ReturnsConsoleIsInputRedirected()
    {
        var service = new ConsoleService();

        var result = service.IsInputRedirected;

        result.ShouldBe(Console.IsInputRedirected);
    }
}
