using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using Cowsay.Abstractions;
using Cowsay.CLI.Abstractions;
using Cowsay.CLI.Application;
using Cowsay.CLI.Configuration;
using Cowsay.CLI.Infrastructure;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using Xunit;

namespace Cowsay.CLI.Tests;

public class CliRunnerTests
{
    private readonly IConsoleService _consoleService;
    private readonly IStdinReader _stdinReader;
    private readonly IHelpTextFormatter _helpTextFormatter;
    private readonly ICowListFormatter _cowListFormatter;
    private readonly CowsayExecutor _cowsayExecutor;
    private readonly ICowFormatProvider _cowFormatProvider;
    private readonly TextWriter _outWriter;
    private readonly TextWriter _errorWriter;

    public CliRunnerTests()
    {
        _consoleService = Substitute.For<IConsoleService>();
        _stdinReader = Substitute.For<IStdinReader>();
        _helpTextFormatter = Substitute.For<IHelpTextFormatter>();
        _cowListFormatter = Substitute.For<ICowListFormatter>();
        _cowFormatProvider = Substitute.For<ICowFormatProvider>();

        var cowLoader = Substitute.For<ICowLoader>();
        _cowsayExecutor = new CowsayExecutor(cowLoader);

        _outWriter = Substitute.For<TextWriter>();
        _errorWriter = Substitute.For<TextWriter>();
        _consoleService.Out.Returns(_outWriter);
        _consoleService.Error.Returns(_errorWriter);
    }

    private CliRunner CreateRunner()
    {
        return new CliRunner(
            _consoleService,
            _stdinReader,
            _helpTextFormatter,
            _cowListFormatter,
            _cowsayExecutor,
            _cowFormatProvider);
    }

    [Fact]
    public async Task RunAsync_WithValidMessage_ReturnsSuccessCode()
    {
        var runner = CreateRunner();
        var cowLoader = Substitute.For<ICowLoader>();
        var cow = Substitute.For<ICow>();
        cow.Speak("Hello", "oo", "  ", 40).Returns("Moo!");
        cowLoader.LoadCowAsync(null, "default").Returns(cow);

        var executor = new CowsayExecutor(cowLoader);
        var testRunner = new CliRunner(
            _consoleService,
            _stdinReader,
            _helpTextFormatter,
            _cowListFormatter,
            executor,
            _cowFormatProvider);

        var result = await testRunner.RunAsync(new[] { "Hello" });

        result.ShouldBe(0);
        await _outWriter.Received(1).WriteLineAsync("Moo!");
    }

    [Fact]
    public async Task RunAsync_WithError_WritesToErrorAndReturnsFailureCode()
    {
        var runner = CreateRunner();
        var cowLoader = Substitute.For<ICowLoader>();
        cowLoader.LoadCowAsync(null, "invalid").Throws(new FileNotFoundException());

        var executor = new CowsayExecutor(cowLoader);
        var testRunner = new CliRunner(
            _consoleService,
            _stdinReader,
            _helpTextFormatter,
            _cowListFormatter,
            executor,
            _cowFormatProvider);

        var result = await testRunner.RunAsync(new[] { "-c", "invalid", "Test" });

        result.ShouldBe(1);
        await _errorWriter.Received(1).WriteLineAsync(Arg.Is<string>(s => s.Contains("not found")));
    }

    [Fact]
    public async Task RunAsync_WithHelpFlag_DisplaysHelpAndReturnsSuccess()
    {
        var runner = CreateRunner();
        var helpText = "Help text";
        _helpTextFormatter.FormatHelp(Arg.Any<ParserResult<Options>>(), Arg.Any<IEnumerable<Error>>())
            .Returns(helpText);

        var result = await runner.RunAsync(new[] { "--help" });

        result.ShouldBe(0);
        await _outWriter.Received(1).WriteLineAsync(helpText);
    }

    [Fact]
    public async Task RunAsync_WithVersionFlag_DisplaysVersionAndReturnsSuccess()
    {
        var runner = CreateRunner();
        var version = "cowsay 1.0.0";
        _helpTextFormatter.FormatVersion().Returns(version);

        var result = await runner.RunAsync(new[] { "--version" });

        result.ShouldBe(0);
        await _outWriter.Received(1).WriteLineAsync(version);
    }

    [Fact]
    public async Task RunAsync_WithInvalidArgs_DisplaysHelpAndReturnsFailure()
    {
        var runner = CreateRunner();
        var helpText = "Error help text";
        _helpTextFormatter.FormatHelp(Arg.Any<ParserResult<Options>>(), Arg.Any<IEnumerable<Error>>())
            .Returns(helpText);

        var result = await runner.RunAsync(new[] { "--invalid-option" });

        result.ShouldBe(1);
        await _outWriter.Received(1).WriteLineAsync(helpText);
    }

    [Fact]
    public async Task RunAsync_WithListFlag_DisplaysCowListAndReturnsSuccess()
    {
        var runner = CreateRunner();
        var cows = new[] { "default", "tux", "dragon" };
        var formattedList = "default\ntux\ndragon";

        _cowFormatProvider.GetAvailableCowsAsync().Returns(cows);
        _cowListFormatter.Format(cows).Returns(formattedList);

        var result = await runner.RunAsync(new[] { "-l" });

        result.ShouldBe(0);
        await _outWriter.Received(1).WriteLineAsync(formattedList);
    }

    [Fact]
    public async Task RunAsync_WithNoMessageAndRedirectedInput_ReadsFromStdin()
    {
        var runner = CreateRunner();
        var stdinMessage = "Message from stdin";
        _consoleService.IsInputRedirected.Returns(true);
        _stdinReader.ReadToEndAsync().Returns(stdinMessage);

        var cowLoader = Substitute.For<ICowLoader>();
        var cow = Substitute.For<ICow>();
        cow.Speak(stdinMessage, "oo", "  ", 40).Returns("Moo from stdin!");
        cowLoader.LoadCowAsync(null, "default").Returns(cow);

        var executor = new CowsayExecutor(cowLoader);
        var testRunner = new CliRunner(
            _consoleService,
            _stdinReader,
            _helpTextFormatter,
            _cowListFormatter,
            executor,
            _cowFormatProvider);

        var result = await testRunner.RunAsync(Array.Empty<string>());

        result.ShouldBe(0);
        await _stdinReader.Received(1).ReadToEndAsync();
        await _outWriter.Received(1).WriteLineAsync("Moo from stdin!");
    }

    [Fact]
    public async Task RunAsync_WithNoMessageAndRedirectedInputWithNewlines_TrimsNewlines()
    {
        var runner = CreateRunner();
        var stdinMessage = "Message from stdin\n\r";
        var trimmedMessage = "Message from stdin";
        _consoleService.IsInputRedirected.Returns(true);
        _stdinReader.ReadToEndAsync().Returns(stdinMessage);

        var cowLoader = Substitute.For<ICowLoader>();
        var cow = Substitute.For<ICow>();
        cow.Speak(trimmedMessage, "oo", "  ", 40).Returns("Moo!");
        cowLoader.LoadCowAsync(null, "default").Returns(cow);

        var executor = new CowsayExecutor(cowLoader);
        var testRunner = new CliRunner(
            _consoleService,
            _stdinReader,
            _helpTextFormatter,
            _cowListFormatter,
            executor,
            _cowFormatProvider);

        var result = await testRunner.RunAsync(Array.Empty<string>());

        result.ShouldBe(0);
        cow.Received(1).Speak(trimmedMessage, "oo", "  ", 40);
    }

    [Fact]
    public async Task RunAsync_WithNoMessageAndNoRedirect_ReturnsError()
    {
        var runner = CreateRunner();
        _consoleService.IsInputRedirected.Returns(false);

        var result = await runner.RunAsync(Array.Empty<string>());

        result.ShouldBe(1);
        await _errorWriter.Received(1).WriteLineAsync(ErrorMessages.NoMessageProvided);
    }

    [Fact]
    public async Task RunAsync_WithNoMessageAndEmptyStdin_ReturnsError()
    {
        var runner = CreateRunner();
        _consoleService.IsInputRedirected.Returns(true);
        _stdinReader.ReadToEndAsync().Returns(string.Empty);

        var result = await runner.RunAsync(Array.Empty<string>());

        result.ShouldBe(1);
        await _errorWriter.Received(1).WriteLineAsync(ErrorMessages.NoMessageProvided);
    }

    [Fact]
    public async Task RunAsync_WithNoMessageAndWhitespaceStdin_ReturnsError()
    {
        var runner = CreateRunner();
        _consoleService.IsInputRedirected.Returns(true);
        _stdinReader.ReadToEndAsync().Returns("\n\r");

        var result = await runner.RunAsync(Array.Empty<string>());

        result.ShouldBe(1);
        await _errorWriter.Received(1).WriteLineAsync(ErrorMessages.NoMessageProvided);
    }

    [Fact]
    public async Task ExecuteAsync_BuildsServiceProviderAndExecutes()
    {
        var args = new[] { "--help" };
        var result = await CliRunner.ExecuteAsync(args);

        result.ShouldBe(0);
    }
}
