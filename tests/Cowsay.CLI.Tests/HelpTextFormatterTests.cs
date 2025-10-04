using System.Collections.Generic;
using System.Linq;
using CommandLine;
using Cowsay.CLI.Formatters;
using Cowsay.CLI.Infrastructure;
using Shouldly;
using Xunit;

namespace Cowsay.CLI.Tests;

public class HelpTextFormatterTests
{
    [Fact]
    public void FormatHelp_WithErrors_ReturnsFormattedHelpText()
    {
        var formatter = new HelpTextFormatter();
        var parser = new Parser(with => with.HelpWriter = null);
        var result = parser.ParseArguments<Options>(new[] { "--invalid" });
        var errors = ((NotParsed<Options>)result).Errors;

        var helpText = formatter.FormatHelp(result, errors);

        helpText.ShouldNotBeNull();
        helpText.ShouldNotBeEmpty();
        helpText.ShouldContain("cowsay CLI");
        helpText.ShouldContain("Usage:");
        helpText.ShouldContain("cowsay \"Your message here\"");
        helpText.ShouldContain("echo \"Piped message\" | cowsay");
        helpText.ShouldContain("Options:");
    }

    [Fact]
    public void FormatHelp_ContainsCowAsciiArt()
    {
        var formatter = new HelpTextFormatter();
        var parser = new Parser(with => with.HelpWriter = null);
        var result = parser.ParseArguments<Options>(new[] { "--help" });
        var errors = ((NotParsed<Options>)result).Errors;

        var helpText = formatter.FormatHelp(result, errors);

        helpText.ShouldContain("^__^");
        helpText.ShouldContain("(oo)");
        helpText.ShouldContain("||----w |");
    }

    [Fact]
    public void FormatHelp_ContainsAllExamples()
    {
        var formatter = new HelpTextFormatter();
        var parser = new Parser(with => with.HelpWriter = null);
        var result = parser.ParseArguments<Options>(new[] { "--help" });
        var errors = ((NotParsed<Options>)result).Errors;

        var helpText = formatter.FormatHelp(result, errors);

        helpText.ShouldContain("Basic usage");
        helpText.ShouldContain("Custom eyes");
        helpText.ShouldContain("Thinking cow");
        helpText.ShouldContain("List all cow types");
        helpText.ShouldContain("\"Hello, World!\"");
        helpText.ShouldContain("--eyes @@");
        helpText.ShouldContain("--think");
        helpText.ShouldContain("--list");
    }

    [Fact]
    public void FormatHelp_WithParsedResult_StillReturnsHelp()
    {
        var formatter = new HelpTextFormatter();
        var parser = new Parser(with => with.HelpWriter = null);
        var result = parser.ParseArguments<Options>(new[] { "Test" });
        var emptyErrors = Enumerable.Empty<Error>();

        var helpText = formatter.FormatHelp(result, emptyErrors);

        helpText.ShouldNotBeNull();
        helpText.ShouldNotBeEmpty();
    }

    [Fact]
    public void FormatVersion_ReturnsVersionString()
    {
        var formatter = new HelpTextFormatter();

        var version = formatter.FormatVersion();

        version.ShouldNotBeNull();
        version.ShouldStartWith("cowsay ");
        version.ShouldContain(".");
    }

    [Fact]
    public void FormatVersion_FollowsExpectedFormat()
    {
        var formatter = new HelpTextFormatter();

        var version = formatter.FormatVersion();

        version.ShouldMatch(@"^cowsay \d+\.\d+\.\d+$");
    }
}
