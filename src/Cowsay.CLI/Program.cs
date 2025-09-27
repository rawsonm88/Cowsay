using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using CommandLine;
using CommandLine.Text;
using Cowsay.Abstractions;

namespace Cowsay.CLI;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var parser = new Parser(with => with.HelpWriter = null);
        var parserResult = parser.ParseArguments<Options>(args);

        return await parserResult
            .MapResult(
                async (Options opts) => await RunOptionsAsync(opts),
                async errs => await DisplayHelp(parserResult, errs));
    }

    private static async Task<int> RunOptionsAsync(Options opts)
    {
        if (opts.List)
        {
            var provider = new EmbeddedCowFormatProvider();
            var cows = await provider.GetAvailableCowsAsync();
            foreach (var cow in cows)
            {
                Console.WriteLine(cow);
            }
            return 0;
        }

        string message = opts.Message;
        if (string.IsNullOrEmpty(message))
        {
            if (Console.IsInputRedirected)
            {
                using var reader = new StreamReader(Console.OpenStandardInput());
                message = await reader.ReadToEndAsync();
                message = message.TrimEnd('\n', '\r');
            }
            else
            {
                await Console.Error.WriteLineAsync("Error: No message provided. Use --help for usage information.");
                return 1;
            }
        }

        try
        {
            ICow cow = await DefaultCattleFarmer.RearCowWithDefaults(opts.Cow);

            string output;
            if (opts.Think)
            {
                output = cow.Think(message, opts.Eyes ?? "oo", opts.Tongue ?? "  ", opts.Wrap ?? 40);
            }
            else
            {
                output = cow.Speak(message, opts.Eyes ?? "oo", opts.Tongue ?? "  ", opts.Wrap ?? 40);
            }

            Console.WriteLine(output);
            return 0;
        }
        catch (FileNotFoundException)
        {
            await Console.Error.WriteLineAsync($"Error: Cow format '{opts.Cow}' not found. Use --list to see available formats.");
            return 1;
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync($"Error: {ex.Message}");
            return 1;
        }
    }
    
        private static async Task<int> DisplayHelp<T>(ParserResult<T> result, IEnumerable<Error> errs)
    {
        var isVersionRequest = errs.Any(e => e.Tag == ErrorType.VersionRequestedError);
        var isHelpRequest = errs.Any(e => e.Tag == ErrorType.HelpRequestedError || e.Tag == ErrorType.HelpVerbRequestedError);

        if (isVersionRequest)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "1.0.0";
            Console.WriteLine($"cowsay {version}");
            return await Task.FromResult(0);
        }

        var helpText = HelpText.AutoBuild(result, h =>
        {
            h.AdditionalNewLineAfterOption = false;
            h.Heading = " _____________";
            h.Copyright = "< cowsay CLI >";
            h.AddPreOptionsLine(" -------------");
            h.AddPreOptionsLine("        \\   ^__^");
            h.AddPreOptionsLine("         \\  (oo)\\_______");
            h.AddPreOptionsLine("            (__)\\       )\\/\\");
            h.AddPreOptionsLine("                ||----w |");
            h.AddPreOptionsLine("                ||     ||");
            h.AddPreOptionsLine("");
            h.AddPreOptionsLine("Usage:");
            h.AddPreOptionsLine("  cowsay \"Your message here\"");
            h.AddPreOptionsLine("  echo \"Piped message\" | cowsay");
            h.AddPreOptionsLine("  fortune | cowsay");
            h.AddPreOptionsLine("");
            h.AddPreOptionsLine("Examples:");
            h.AddPreOptionsLine("  cowsay \"Hello, World!\"         # Basic usage");
            h.AddPreOptionsLine("  cowsay -e @@ \"I see you\"       # Custom eyes");
            h.AddPreOptionsLine("  cowsay -T \"Hmm...\"             # Thinking cow");
            h.AddPreOptionsLine("  cowsay -l                       # List all cow types");
            h.AddPreOptionsLine("");
            h.AddPreOptionsLine("Options:");
            h.AutoVersion = false;
            h.AutoHelp = false;
            h.AddDashesToOption = true;
            return HelpText.DefaultParsingErrorsHandler(result, h);
        }, e => e, verbsIndex: true);

        Console.WriteLine(helpText);
        return await Task.FromResult(isHelpRequest ? 0 : 1);
    }
}