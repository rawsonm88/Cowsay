using System.Collections.Generic;
using System.Reflection;
using CommandLine;
using CommandLine.Text;
using Cowsay.CLI.Abstractions;

namespace Cowsay.CLI.Formatters;

public class HelpTextFormatter : IHelpTextFormatter
{
    public string FormatHelp<T>(ParserResult<T> result, IEnumerable<Error> errors)
    {
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
            h.AddPreOptionsLine("Options:");
            h.AutoVersion = false;
            h.AutoHelp = false;
            h.AddDashesToOption = true;
            return HelpText.DefaultParsingErrorsHandler(result, h);
        }, e => e, verbsIndex: true);

        return helpText;
    }

    public string FormatVersion()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "1.0.0";
        return $"cowsay {version}";
    }
}
