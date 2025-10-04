namespace Cowsay.CLI.Application;

public class CowsayResult
{
    public int ExitCode { get; init; }
    public string? Output { get; init; }
    public string? Error { get; init; }

    public static CowsayResult Success(string output) =>
        new() { ExitCode = 0, Output = output };

    public static CowsayResult Failure(string error, int exitCode = 1) =>
        new() { ExitCode = exitCode, Error = error };
}
