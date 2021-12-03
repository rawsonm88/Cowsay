using FluentAssertions;
using Xunit;

namespace Cowsay.UnitTests
{
    public partial class RegularExpressionTests
    {
        [Theory]
        [InlineData("$eye", "@@@", "@@@")]
        [InlineData("$eyeotherthings", "[[[", "[[[otherthings")]
        [InlineData("other$eyethings", "...", "other...things")]
        [InlineData("other$eye", ";;;", "other;;;")]
        public void Replace_eye_placeholder(string input, string replacement, string expectedOutput)
        {
            string output = RegularExpressions.Eye.Replace(input, replacement);

            output.Should().Be(expectedOutput);
        }

        [Theory]
        [InlineData("", "@@@")]
        [InlineData("otherthings", "[[[")]
        [InlineData("other$things", "...")]
        [InlineData("$other", ";;;")]
        public void Return_original_text_if_no_eye_placeholder(string input, string replacement)
        {
            string output = RegularExpressions.Eye.Replace(input, replacement);

            output.Should().Be(input);
        }
    }
}