using System;
using System.IO;
using System.Threading.Tasks;
using CommandLine;
using Cowsay.CLI;
using Shouldly;
using Xunit;

namespace Cowsay.CLI.Tests;

public class CommandLineParsingTests
{
    [Fact]
    public void BasicMessage_ShouldParse()
    {
        var args = new[] { "Hello, World!" };
        var parser = new Parser(with => with.HelpWriter = null);
        var result = parser.ParseArguments<Options>(args);

        result.Tag.ShouldBe(ParserResultType.Parsed);
        ((Parsed<Options>)result).Value.Message.ShouldBe("Hello, World!");
    }

    [Fact]
    public void CustomEyes_ShouldParse()
    {
        var args = new[] { "-e", "@@", "Test" };
        var parser = new Parser(with => with.HelpWriter = null);
        var result = parser.ParseArguments<Options>(args);

        result.Tag.ShouldBe(ParserResultType.Parsed);
        var options = ((Parsed<Options>)result).Value;
        options.Eyes.ShouldBe("@@");
        options.Message.ShouldBe("Test");
    }

    [Fact]
    public void ThinkMode_ShouldParse()
    {
        var args = new[] { "-T", "Thinking" };
        var parser = new Parser(with => with.HelpWriter = null);
        var result = parser.ParseArguments<Options>(args);

        result.Tag.ShouldBe(ParserResultType.Parsed);
        var options = ((Parsed<Options>)result).Value;
        options.Think.ShouldBeTrue();
        options.Message.ShouldBe("Thinking");
    }

    [Fact]
    public void ListFlag_ShouldParse()
    {
        var args = new[] { "-l" };
        var parser = new Parser(with => with.HelpWriter = null);
        var result = parser.ParseArguments<Options>(args);

        result.Tag.ShouldBe(ParserResultType.Parsed);
        var options = ((Parsed<Options>)result).Value;
        options.List.ShouldBeTrue();
    }

    [Fact]
    public void WrapOption_ShouldParse()
    {
        var args = new[] { "-w", "20", "Message" };
        var parser = new Parser(with => with.HelpWriter = null);
        var result = parser.ParseArguments<Options>(args);

        result.Tag.ShouldBe(ParserResultType.Parsed);
        var options = ((Parsed<Options>)result).Value;
        options.Wrap.ShouldBe(20);
        options.Message.ShouldBe("Message");
    }

    [Fact]
    public void CustomCow_ShouldParse()
    {
        var args = new[] { "-c", "tux", "Message" };
        var parser = new Parser(with => with.HelpWriter = null);
        var result = parser.ParseArguments<Options>(args);

        result.Tag.ShouldBe(ParserResultType.Parsed);
        var options = ((Parsed<Options>)result).Value;
        options.Cow.ShouldBe("tux");
        options.Message.ShouldBe("Message");
    }

    [Fact]
    public void MultipleOptions_ShouldParse()
    {
        var args = new[] { "-e", "xx", "-t", "U ", "-w", "30", "-c", "default", "Test message" };
        var parser = new Parser(with => with.HelpWriter = null);
        var result = parser.ParseArguments<Options>(args);

        result.Tag.ShouldBe(ParserResultType.Parsed);
        var options = ((Parsed<Options>)result).Value;
        options.Eyes.ShouldBe("xx");
        options.Tongue.ShouldBe("U ");
        options.Wrap.ShouldBe(30);
        options.Cow.ShouldBe("default");
        options.Message.ShouldBe("Test message");
    }

    [Fact]
    public void HelpFlag_ShouldTriggerHelp()
    {
        var args = new[] { "--help" };
        var parser = new Parser(with => with.HelpWriter = null);
        var result = parser.ParseArguments<Options>(args);

        result.Tag.ShouldBe(ParserResultType.NotParsed);
        ((NotParsed<Options>)result).Errors
            .ShouldContain(e => e.Tag == CommandLine.ErrorType.HelpRequestedError);
    }
}