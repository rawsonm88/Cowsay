using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Cowsay.Abstractions;
using Cowsay.CLI.Abstractions;
using Cowsay.CLI.Application;
using Cowsay.CLI.Infrastructure;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using Xunit;

namespace Cowsay.CLI.Tests;

public class CowsayExecutorTests
{
    [Fact]
    public async Task ExecuteAsync_SuccessfulSpeech_ReturnsSuccessResult()
    {
        var cowLoader = Substitute.For<ICowLoader>();
        var cow = Substitute.For<ICow>();
        cow.Speak("Hello", "oo", "  ", 40).Returns("Cow says Hello");
        cowLoader.LoadCowAsync(null, "default").Returns(cow);

        var executor = new CowsayExecutor(cowLoader);
        var options = new Options { Cow = "default", Think = false };

        var result = await executor.ExecuteAsync(options, "Hello");

        result.ExitCode.ShouldBe(0);
        result.Output.ShouldBe("Cow says Hello");
        result.Error.ShouldBeNull();
    }

    [Fact]
    public async Task ExecuteAsync_SuccessfulThink_ReturnsSuccessResult()
    {
        var cowLoader = Substitute.For<ICowLoader>();
        var cow = Substitute.For<ICow>();
        cow.Think("Hmm", "oo", "  ", 40).Returns("Cow thinks Hmm");
        cowLoader.LoadCowAsync(null, "default").Returns(cow);

        var executor = new CowsayExecutor(cowLoader);
        var options = new Options { Cow = "default", Think = true };

        var result = await executor.ExecuteAsync(options, "Hmm");

        result.ExitCode.ShouldBe(0);
        result.Output.ShouldBe("Cow thinks Hmm");
        result.Error.ShouldBeNull();
    }

    [Fact]
    public async Task ExecuteAsync_WithCustomEyes_PassesToCow()
    {
        var cowLoader = Substitute.For<ICowLoader>();
        var cow = Substitute.For<ICow>();
        cow.Speak("Test", "@@", "  ", 40).Returns("Cow with @@ eyes");
        cowLoader.LoadCowAsync(null, "default").Returns(cow);

        var executor = new CowsayExecutor(cowLoader);
        var options = new Options { Cow = "default", Eyes = "@@" };

        var result = await executor.ExecuteAsync(options, "Test");

        result.ExitCode.ShouldBe(0);
        cow.Received(1).Speak("Test", "@@", "  ", 40);
    }

    [Fact]
    public async Task ExecuteAsync_WithCustomWrap_PassesToCow()
    {
        var cowLoader = Substitute.For<ICowLoader>();
        var cow = Substitute.For<ICow>();
        cow.Speak("Test", "oo", "  ", 20).Returns("Wrapped cow");
        cowLoader.LoadCowAsync(null, "default").Returns(cow);

        var executor = new CowsayExecutor(cowLoader);
        var options = new Options { Cow = "default", Wrap = 20 };

        var result = await executor.ExecuteAsync(options, "Test");

        result.ExitCode.ShouldBe(0);
        cow.Received(1).Speak("Test", "oo", "  ", 20);
    }

    [Fact]
    public async Task ExecuteAsync_FileNotFound_WithFile_ReturnsFileErrorMessage()
    {
        var cowLoader = Substitute.For<ICowLoader>();
        cowLoader.LoadCowAsync("missing.cow", "default").Throws(new FileNotFoundException());

        var executor = new CowsayExecutor(cowLoader);
        var options = new Options { File = "missing.cow", Cow = "default" };

        var result = await executor.ExecuteAsync(options, "Test");

        result.ExitCode.ShouldBe(1);
        result.Output.ShouldBeNull();
        result.Error.ShouldBe("Error: File 'missing.cow' not found.");
    }

    [Fact]
    public async Task ExecuteAsync_FileNotFound_WithoutFile_ReturnsCowFormatErrorMessage()
    {
        var cowLoader = Substitute.For<ICowLoader>();
        cowLoader.LoadCowAsync(null, "invalid").Throws(new FileNotFoundException());

        var executor = new CowsayExecutor(cowLoader);
        var options = new Options { Cow = "invalid" };

        var result = await executor.ExecuteAsync(options, "Test");

        result.ExitCode.ShouldBe(1);
        result.Output.ShouldBeNull();
        result.Error.ShouldBe("Error: Cow format 'invalid' not found. Use --list to see available formats.");
    }

    [Fact]
    public async Task ExecuteAsync_HttpRequestException_ReturnsUrlErrorMessage()
    {
        var cowLoader = Substitute.For<ICowLoader>();
        var httpException = new HttpRequestException("404 Not Found");
        cowLoader.LoadCowAsync("http://example.com/cow.cow", "default").Throws(httpException);

        var executor = new CowsayExecutor(cowLoader);
        var options = new Options { File = "http://example.com/cow.cow", Cow = "default" };

        var result = await executor.ExecuteAsync(options, "Test");

        result.ExitCode.ShouldBe(1);
        result.Output.ShouldBeNull();
        result.Error.ShouldNotBeNull();
        result.Error.ShouldContain("Failed to fetch URL");
        result.Error.ShouldContain("404 Not Found");
    }

    [Fact]
    public async Task ExecuteAsync_GenericException_ReturnsGenericErrorMessage()
    {
        var cowLoader = Substitute.For<ICowLoader>();
        cowLoader.LoadCowAsync(null, "default").Throws(new InvalidOperationException("Something went wrong"));

        var executor = new CowsayExecutor(cowLoader);
        var options = new Options { Cow = "default" };

        var result = await executor.ExecuteAsync(options, "Test");

        result.ExitCode.ShouldBe(1);
        result.Output.ShouldBeNull();
        result.Error.ShouldBe("Error: Something went wrong");
    }
}
