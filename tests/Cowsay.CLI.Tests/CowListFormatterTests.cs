using System;
using System.Linq;
using Cowsay.CLI.Formatters;
using Shouldly;
using Xunit;

namespace Cowsay.CLI.Tests;

public class CowListFormatterTests
{
    [Fact]
    public void Format_WithMultipleCows_JoinsWithNewlines()
    {
        var formatter = new CowListFormatter();
        var cows = new[] { "default", "tux", "dragon" };

        var result = formatter.Format(cows);

        result.ShouldBe("default\ntux\ndragon");
    }

    [Fact]
    public void Format_WithSingleCow_ReturnsSingleLine()
    {
        var formatter = new CowListFormatter();
        var cows = new[] { "default" };

        var result = formatter.Format(cows);

        result.ShouldBe("default");
    }

    [Fact]
    public void Format_WithEmptyList_ReturnsEmptyString()
    {
        var formatter = new CowListFormatter();
        var cows = Array.Empty<string>();

        var result = formatter.Format(cows);

        result.ShouldBe(string.Empty);
    }

    [Fact]
    public void Format_UsesEnvironmentNewLine()
    {
        var formatter = new CowListFormatter();
        var cows = new[] { "cow1", "cow2" };

        var result = formatter.Format(cows);

        var expectedNewline = Environment.NewLine;
        result.ShouldContain(expectedNewline);
        result.Split(expectedNewline).Length.ShouldBe(2);
    }

    [Fact]
    public void Format_PreservesOrder()
    {
        var formatter = new CowListFormatter();
        var cows = new[] { "zebra", "ant", "monkey" };

        var result = formatter.Format(cows);

        var lines = result.Split(Environment.NewLine);
        lines[0].ShouldBe("zebra");
        lines[1].ShouldBe("ant");
        lines[2].ShouldBe("monkey");
    }
}
