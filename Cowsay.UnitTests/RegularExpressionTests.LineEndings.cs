using FluentAssertions;
using Xunit;

namespace Cowsay.UnitTests
{
    public partial class RegularExpressionTests
    {
        [Theory]
        [InlineData("$e\nye", "@@@", "$e@@@ye")]
        [InlineData("$eye\r\notherthi\rngs", "[[[", "$eye[[[otherthi[[[ngs")]
        [InlineData("other$eye\nthings", "...", "other$eye...things")]
        [InlineData("other\r\n$eye\r\n", "\n", "other\n$eye\n")]
        public void Replace_all_new_line_chars(string input, string replacement, string expectedOutput)
        {
            string output = RegularExpressions.LineEndings.Replace(input, replacement);

            output.Should().Be(expectedOutput);
        }

        [Theory]
        [InlineData("", "@@@")]
        [InlineData("otherthings", "[[[")]
        [InlineData("other$things", "...")]
        [InlineData("$other", ";;;")]
        public void Return_original_text_if_no_new_lines(string input, string replacement)
        {
            string output = RegularExpressions.LineEndings.Replace(input, replacement);

            output.Should().Be(input);
        }
    }
}