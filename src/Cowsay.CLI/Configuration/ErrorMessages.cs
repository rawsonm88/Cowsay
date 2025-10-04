namespace Cowsay.CLI.Configuration;

public static class ErrorMessages
{
    public const string NoMessageProvided = "Error: No message provided. Use --help for usage information.";

    public static string FileNotFound(string filePath) =>
        $"Error: File '{filePath}' not found.";

    public static string CowFormatNotFound(string cowName) =>
        $"Error: Cow format '{cowName}' not found. Use --list to see available formats.";

    public static string UrlFetchFailed(string url, string message) =>
        $"Error: Failed to fetch URL '{url}': {message}";

    public static string UnexpectedError(string message) =>
        $"Error: {message}";
}
