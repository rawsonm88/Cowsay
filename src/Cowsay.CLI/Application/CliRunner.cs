using CommandLine;
using Cowsay.Abstractions;
using Cowsay.CLI.Abstractions;
using Cowsay.CLI.Configuration;
using Cowsay.CLI.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Cowsay.CLI.Application;

public class CliRunner
{
    private readonly IConsoleService _consoleService;
    private readonly IStdinReader _stdinReader;
    private readonly IHelpTextFormatter _helpTextFormatter;
    private readonly ICowListFormatter _cowListFormatter;
    private readonly CowsayExecutor _cowsayExecutor;
    private readonly ICowFormatProvider _cowFormatProvider;

    public CliRunner(
        IConsoleService consoleService,
        IStdinReader stdinReader,
        IHelpTextFormatter helpTextFormatter,
        ICowListFormatter cowListFormatter,
        CowsayExecutor cowsayExecutor,
        ICowFormatProvider cowFormatProvider)
    {
        _consoleService = consoleService;
        _stdinReader = stdinReader;
        _helpTextFormatter = helpTextFormatter;
        _cowListFormatter = cowListFormatter;
        _cowsayExecutor = cowsayExecutor;
        _cowFormatProvider = cowFormatProvider;
    }

    public static async Task<int> ExecuteAsync(string[] args)
    {
        using var serviceProvider = new ServiceCollection()
            .AddCowsayCliServices()
            .BuildServiceProvider();

        var runner = serviceProvider.GetRequiredService<CliRunner>();
        return await runner.RunAsync(args);
    }

    public async Task<int> RunAsync(string[] args)
    {
        var parser = new Parser(with => with.HelpWriter = null);
        var parserResult = parser.ParseArguments<Options>(args);

        return await parserResult
            .MapResult(
                async (Options opts) => await ExecuteOptionsAsync(opts),
                async errs => await DisplayHelpAsync(parserResult, errs));
    }

    private async Task<int> ExecuteOptionsAsync(Options options)
    {
        var result = await RunCowsayAsync(options);

        if (result.ExitCode == 0)
        {
            await _consoleService.Out.WriteLineAsync(result.Output);
        }
        else
        {
            await _consoleService.Error.WriteLineAsync(result.Error);
        }

        return result.ExitCode;
    }

    private async Task<int> DisplayHelpAsync<T>(ParserResult<T> result, IEnumerable<Error> errors)
    {
        var isVersionRequest = errors.Any(e => e.Tag == ErrorType.VersionRequestedError);
        var isHelpRequest = errors.Any(e => e.Tag == ErrorType.HelpRequestedError || e.Tag == ErrorType.HelpVerbRequestedError);

        if (isVersionRequest)
        {
            await _consoleService.Out.WriteLineAsync(_helpTextFormatter.FormatVersion());
            return 0;
        }

        var helpText = _helpTextFormatter.FormatHelp(result, errors);
        await _consoleService.Out.WriteLineAsync(helpText);
        return isHelpRequest ? 0 : 1;
    }

    private async Task<CowsayResult> RunCowsayAsync(Options options)
    {
        if (options.List)
        {
            return await ListCowsAsync();
        }

        var message = options.Message;
        if (string.IsNullOrEmpty(message))
        {
            if (_consoleService.IsInputRedirected)
            {
                message = await _stdinReader.ReadToEndAsync();
                message = message.TrimEnd('\n', '\r');
            }

            if (string.IsNullOrEmpty(message))
            {
                return CowsayResult.Failure(ErrorMessages.NoMessageProvided);
            }
        }

        return await _cowsayExecutor.ExecuteAsync(options, message);
    }

    private async Task<CowsayResult> ListCowsAsync()
    {
        var cows = await _cowFormatProvider.GetAvailableCowsAsync();
        var output = _cowListFormatter.Format(cows);
        return CowsayResult.Success(output);
    }
}
