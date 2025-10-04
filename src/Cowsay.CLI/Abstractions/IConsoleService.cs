using System.IO;

namespace Cowsay.CLI.Abstractions;

public interface IConsoleService
{
    bool IsInputRedirected { get; }
    TextWriter Out { get; }
    TextWriter Error { get; }
}
