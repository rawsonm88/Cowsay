using CommandLine;
using CommandLine.Text;

namespace Cowsay.CLI.Infrastructure;

public class Options
{
    [Usage(ApplicationAlias = "cowsay")]
    public static IEnumerable<Example> Examples
    {
        get
        {
            yield return new Example("Basic usage", new Options { Message = "Hello, World!" });
            yield return new Example("Custom eyes", new Options { Message = "I see you", Eyes = "@@" });
            yield return new Example("Thinking cow", new Options { Message = "Hmm...", Think = true });
            yield return new Example("List all cow types", new Options { List = true });
        }
    }

    [Value(0, MetaName = "MESSAGE", HelpText = "The message for the cow to say (or pipe from stdin)")]
    public string Message { get; set; } = string.Empty;

    [Option('l', "list", HelpText = "List all available cow formats")]
    public bool List { get; set; }

    [Option('c', "cow", Default = "default", HelpText = "Choose a cow format (use -l to see all)")]
    public string Cow { get; set; } = "default";

    [Option('f', "file", HelpText = "Load cow from a .cow file or URL")]
    public string? File { get; set; }

    [Option('e', "eyes", HelpText = "Set the cow's eyes (2 chars, e.g., '@@', 'xx', '$$')")]
    public string? Eyes { get; set; }

    [Option('t', "tongue", HelpText = "Set the cow's tongue (2 chars, e.g., 'U ', '~ ')")]
    public string? Tongue { get; set; }

    [Option('w', "wrap", HelpText = "Wrap text at N columns (default: 40)")]
    public int? Wrap { get; set; }

    [Option('T', "think", HelpText = "Make the cow think instead of say")]
    public bool Think { get; set; }

}