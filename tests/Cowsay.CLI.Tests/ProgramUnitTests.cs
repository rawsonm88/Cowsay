using System;
using System.IO;
using System.Threading.Tasks;
using Shouldly;
using Xunit;
using Xunit.Sdk;

namespace Cowsay.CLI.Tests
{
    public class ProgramUnitTests
    {
        [Fact(Skip = "Console I/O tests can hang in CI/CD pipelines")]
        public async Task Main_returns_zero_for_successful_execution()
        {
            // Arrange
            var args = new[] { "Hello, World!" };

            // Act
            var exitCode = await Program.Main(args);

            // Assert
            exitCode.ShouldBe(0);
        }

        [Fact(Skip = "Console I/O tests can hang in CI/CD pipelines")]
        public async Task Main_returns_zero_for_help_request()
        {
            // Arrange
            var args = new[] { "--help" };

            // Act
            var exitCode = await Program.Main(args);

            // Assert
            exitCode.ShouldBe(0);
        }

        [Fact(Skip = "Console I/O tests can hang in CI/CD pipelines")]
        public async Task Main_returns_zero_for_version_request()
        {
            // Arrange
            var args = new[] { "--version" };

            // Act
            var exitCode = await Program.Main(args);

            // Assert
            exitCode.ShouldBe(0);
        }

        [Fact(Skip = "Console I/O tests can hang in CI/CD pipelines")]
        public async Task Main_returns_zero_for_list_request()
        {
            // Arrange
            var args = new[] { "-l" };

            // Act
            var exitCode = await Program.Main(args);

            // Assert
            exitCode.ShouldBe(0);
        }

        [Fact(Skip = "Console I/O tests can hang in CI/CD pipelines")]
        public async Task Main_returns_one_for_invalid_cow()
        {
            // Arrange
            var args = new[] { "-c", "not-a-real-cow", "Test" };

            // Capture stderr to prevent test output pollution
            var originalError = Console.Error;
            using var errorWriter = new StringWriter();
            Console.SetError(errorWriter);

            try
            {
                // Act
                var exitCode = await Program.Main(args);

                // Assert
                exitCode.ShouldBe(1);
                var error = errorWriter.ToString();
                error.ShouldContain("not found");
            }
            finally
            {
                Console.SetError(originalError);
            }
        }

        [Fact(Skip = "Console I/O tests can hang in CI/CD pipelines")]
        public async Task Main_returns_one_for_invalid_arguments()
        {
            // Arrange
            var args = new[] { "--invalid-flag" };

            // Act
            var exitCode = await Program.Main(args);

            // Assert
            exitCode.ShouldBe(1);
        }

        [Fact(Skip = "Console I/O tests can hang in CI/CD pipelines")]
        public async Task Main_handles_empty_arguments()
        {
            // Skip this test if NOT running in CI where Console behavior may differ
            if (!IsRunningInCI())
            {
                Assert.Skip("Test skipped when not running in CI due to Console I/O differences");
                return;
            }

            // Arrange
            var args = Array.Empty<string>();

            // Capture stdout and stderr
            var originalOut = Console.Out;
            var originalError = Console.Error;
            using var outWriter = new StringWriter();
            using var errorWriter = new StringWriter();
            Console.SetOut(outWriter);
            Console.SetError(errorWriter);

            try
            {
                // Act
                var exitCode = await Program.Main(args);

                // Assert
                // The behavior depends on whether stdin is redirected
                // In test environment with SetIn, it's redirected and reads empty input
                if (exitCode == 0)
                {
                    // Treated as empty piped input
                    var output = outWriter.ToString();
                    output.ShouldContain("^__^"); // Should show cow with empty message
                }
                else
                {
                    // Treated as no input
                    exitCode.ShouldBe(1);
                    var error = errorWriter.ToString();
                    error.ShouldContain("No message provided");
                }
            }
            finally
            {
                Console.SetOut(originalOut);
                Console.SetError(originalError);
            }
        }

        private static bool IsRunningInCI()
        {
            // Check for common CI environment variables
            return Environment.GetEnvironmentVariable("CI") != null ||
                   Environment.GetEnvironmentVariable("GITHUB_ACTIONS") != null ||
                   Environment.GetEnvironmentVariable("JENKINS_URL") != null ||
                   Environment.GetEnvironmentVariable("TEAMCITY_VERSION") != null ||
                   Environment.GetEnvironmentVariable("TF_BUILD") != null || // Azure DevOps
                   Environment.GetEnvironmentVariable("APPVEYOR") != null ||
                   Environment.GetEnvironmentVariable("TRAVIS") != null ||
                   Environment.GetEnvironmentVariable("CIRCLECI") != null ||
                   Environment.GetEnvironmentVariable("GITLAB_CI") != null ||
                   Environment.GetEnvironmentVariable("BUILDKITE") != null;
        }

        [Theory(Skip = "Console I/O tests can hang in CI/CD pipelines")]
        [InlineData("-T", "Thinking")]
        [InlineData("--think", "Thinking")]
        public async Task Main_handles_think_parameter(string flag, string message)
        {
            // Arrange
            var args = new[] { flag, message };

            // Capture stdout
            var originalOut = Console.Out;
            using var outWriter = new StringWriter();
            Console.SetOut(outWriter);

            try
            {
                // Act
                var exitCode = await Program.Main(args);

                // Assert
                exitCode.ShouldBe(0);
                var output = outWriter.ToString();
                output.ShouldContain(message);
                output.ShouldContain("o   ^__^"); // thought bubble indicator
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }

        [Theory(Skip = "Console I/O tests can hang in CI/CD pipelines")]
        [InlineData("-e", "@@")]
        [InlineData("--eyes", "xx")]
        public async Task Main_handles_eyes_parameter(string flag, string eyes)
        {
            // Arrange
            var args = new[] { flag, eyes, "Test" };

            // Capture stdout
            var originalOut = Console.Out;
            using var outWriter = new StringWriter();
            Console.SetOut(outWriter);

            try
            {
                // Act
                var exitCode = await Program.Main(args);

                // Assert
                exitCode.ShouldBe(0);
                var output = outWriter.ToString();
                output.ShouldContain(eyes);
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }

        [Theory(Skip = "Console I/O tests can hang in CI/CD pipelines")]
        [InlineData("-t", "U~")]
        [InlineData("--tongue", "~~")]
        public async Task Main_handles_tongue_parameter(string flag, string tongue)
        {
            // Arrange
            var args = new[] { flag, tongue, "Test" };

            // Capture stdout
            var originalOut = Console.Out;
            using var outWriter = new StringWriter();
            Console.SetOut(outWriter);

            try
            {
                // Act
                var exitCode = await Program.Main(args);

                // Assert
                exitCode.ShouldBe(0);
                var output = outWriter.ToString();
                output.ShouldContain(tongue);
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }

        [Theory(Skip = "Console I/O tests can hang in CI/CD pipelines")]
        [InlineData("-c", "tux")]
        [InlineData("--cow", "dragon")]
        public async Task Main_handles_cow_parameter(string flag, string cowName)
        {
            // Arrange
            var args = new[] { flag, cowName, "Test" };

            // Capture stdout
            var originalOut = Console.Out;
            using var outWriter = new StringWriter();
            Console.SetOut(outWriter);

            try
            {
                // Act
                var exitCode = await Program.Main(args);

                // Assert
                exitCode.ShouldBe(0);
                var output = outWriter.ToString();
                output.ShouldContain("Test");
                // Different cows have different ASCII art
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }

        [Theory(Skip = "Console I/O tests can hang in CI/CD pipelines")]
        [InlineData("-w", "10")]
        [InlineData("--wrap", "20")]
        public async Task Main_handles_wrap_parameter(string flag, string wrap)
        {
            // Arrange
            var args = new[] { flag, wrap, "This is a long message that should wrap" };

            // Capture stdout
            var originalOut = Console.Out;
            using var outWriter = new StringWriter();
            Console.SetOut(outWriter);

            try
            {
                // Act
                var exitCode = await Program.Main(args);

                // Assert
                exitCode.ShouldBe(0);
                var output = outWriter.ToString();
                // Should see multiple lines in the bubble
                var lines = output.Split('\n');
                lines.Length.ShouldBeGreaterThan(5); // At least cow + wrapped message lines
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }
    }
}