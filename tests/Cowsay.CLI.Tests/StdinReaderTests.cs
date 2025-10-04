using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Cowsay.CLI.Infrastructure;
using Shouldly;
using Xunit;

namespace Cowsay.CLI.Tests;

public class StdinReaderTests
{
    [Fact(Skip = "Stdin tests hang in CI")]
    public async Task ReadToEndAsync_ReadsFromStandardInput()
    {
        var originalStdin = Console.In;
        var testInput = "Test input from stdin";

        try
        {
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(testInput));
            var streamReader = new StreamReader(memoryStream);
            Console.SetIn(streamReader);

            var reader = new StdinReader();
            var result = await reader.ReadToEndAsync();

            result.ShouldBe(testInput);
        }
        finally
        {
            Console.SetIn(originalStdin);
        }
    }

    [Fact(Skip = "Stdin tests hang in CI")]
    public async Task ReadToEndAsync_WithEmptyInput_ReturnsEmptyString()
    {
        var originalStdin = Console.In;

        try
        {
            var memoryStream = new MemoryStream(Array.Empty<byte>());
            var streamReader = new StreamReader(memoryStream);
            Console.SetIn(streamReader);

            var reader = new StdinReader();
            var result = await reader.ReadToEndAsync();

            result.ShouldBe(string.Empty);
        }
        finally
        {
            Console.SetIn(originalStdin);
        }
    }

    [Fact(Skip = "Stdin tests hang in CI")]
    public async Task ReadToEndAsync_WithMultilineInput_ReadsAll()
    {
        var originalStdin = Console.In;
        var testInput = "Line 1\nLine 2\nLine 3";

        try
        {
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(testInput));
            var streamReader = new StreamReader(memoryStream);
            Console.SetIn(streamReader);

            var reader = new StdinReader();
            var result = await reader.ReadToEndAsync();

            result.ShouldBe(testInput);
        }
        finally
        {
            Console.SetIn(originalStdin);
        }
    }
}
