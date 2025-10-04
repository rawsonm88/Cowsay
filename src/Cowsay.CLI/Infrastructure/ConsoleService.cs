using System;
using System.IO;
using Cowsay.CLI.Abstractions;

namespace Cowsay.CLI.Infrastructure;

public class ConsoleService : IConsoleService
{
    public bool IsInputRedirected => Console.IsInputRedirected;
    public TextWriter Out => Console.Out;
    public TextWriter Error => Console.Error;
}
