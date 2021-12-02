using FluentAssertions;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Cowsay.UnitTests
{
    public partial class RegularExpressionTests
    {
        [Theory]
        [InlineData("default.cow", "default.txt")]
        [InlineData("bill-the-cat.cow", "bill-the-cat.txt")]
        public async Task Cow_capture_group_only_contains_ascii_art(string cowFileName, string expectedOutputFileName)
        {
            var cowFile = await File.ReadAllTextAsync(Path.Combine("TestCows", cowFileName));
            var match = RegularExpressions.Cow.Match(cowFile);

            var expectedOutput = await File.ReadAllTextAsync(Path.Combine("ExpectedOutputCows", expectedOutputFileName));
            match.Groups["cow"].Value.Should().Be(expectedOutput);
        }
    }
}