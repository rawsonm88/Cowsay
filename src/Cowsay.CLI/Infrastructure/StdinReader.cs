using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using Cowsay.CLI.Abstractions;

namespace Cowsay.CLI.Infrastructure;

[ExcludeFromCodeCoverage(Justification = "Stdin input cannot be reliably tested in automated tests as it hangs waiting for input in CI/CD pipelines. The IStdinReader interface allows this implementation to be mocked in tests.")]
public class StdinReader : IStdinReader
{
    public async Task<string> ReadToEndAsync()
    {
        using var reader = new StreamReader(Console.OpenStandardInput());
        return await reader.ReadToEndAsync();
    }
}
