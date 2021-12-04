using Cowsay.Abstractions;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Cowsay.UnitTests
{
    public class CowTests
    {
        [Theory]
        [InlineData("$eye$tongue$thoughts$eye", @"<A bubble>ACD\B")]
        [InlineData("$eyes$tongue$thoughts", @"<A bubble>ABCD\")]
        public void Calling_say_replaces_the_placeholders_and_includes_the_speech_bubble(string format, string expectedOutput)
        {
            var bubbleGenerator = Substitute.For<IBubbleBlower>();

            bubbleGenerator
                .GetBubble("Hello world", 10)
                .Returns("<A bubble>");

            var cow = new Cow(cowFormat: format, bubbleGenerator);

            var output = cow.Say("Hello world", cowEyes: "AB", cowTongue: "CD", maxCols: 10, isThought: false);

            output.Should().Be(expectedOutput);
        }

        [Theory]
        [InlineData("$eye$tongue$thoughts$eye", @"<A bubble>ACDoB")]
        [InlineData("$eyes$tongue$thoughts", @"<A bubble>ABCDo")]
        public void Calling_say_replaces_the_placeholders_and_includes_the_thought_bubble(string format, string expectedOutput)
        {
            var bubbleGenerator = Substitute.For<IBubbleBlower>();

            bubbleGenerator
                .GetBubble("Hello world", 10, isThought: true)
                .Returns("<A bubble>");

            var cow = new Cow(cowFormat: format, bubbleGenerator);

            var output = cow.Say("Hello world", cowEyes: "AB", cowTongue: "CD", maxCols: 10, isThought: true);

            output.Should().Be(expectedOutput);
        }
    }
}
