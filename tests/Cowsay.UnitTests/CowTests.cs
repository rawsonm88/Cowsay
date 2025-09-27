using Cowsay.Abstractions;
using Shouldly;
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

#pragma warning disable CS0618 // Type or member is obsolete
            var output = cow.Say("Hello world", cowEyes: "AB", cowTongue: "CD", maxCols: 10, isThought: false);
#pragma warning restore CS0618 // Type or member is obsolete

            output.ShouldBe(expectedOutput);
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

#pragma warning disable CS0618 // Type or member is obsolete
            var output = cow.Say("Hello world", cowEyes: "AB", cowTongue: "CD", maxCols: 10, isThought: true);
#pragma warning restore CS0618 // Type or member is obsolete

            output.ShouldBe(expectedOutput);
        }

        [Theory]
        [InlineData("$eye$tongue$thoughts$eye", @"<A bubble>ACD\B")]
        [InlineData("$eyes$tongue$thoughts", @"<A bubble>ABCD\")]
        public void Calling_speak_replaces_the_placeholders_and_includes_the_speech_bubble(string format, string expectedOutput)
        {
            var bubbleGenerator = Substitute.For<IBubbleBlower>();

            bubbleGenerator
                .GetBubble("Hello world", 10)
                .Returns("<A bubble>");

            var cow = new Cow(cowFormat: format, bubbleGenerator);

            var output = cow.Speak("Hello world", cowEyes: "AB", cowTongue: "CD", maxCols: 10);

            output.ShouldBe(expectedOutput);
        }

        [Theory]
        [InlineData("$eye$tongue$thoughts$eye", @"<A bubble>ACDoB")]
        [InlineData("$eyes$tongue$thoughts", @"<A bubble>ABCDo")]
        public void Calling_think_replaces_the_placeholders_and_includes_the_thought_bubble(string format, string expectedOutput)
        {
            var bubbleGenerator = Substitute.For<IBubbleBlower>();

            bubbleGenerator
                .GetBubble("Hello world", 10, isThought: true)
                .Returns("<A bubble>");

            var cow = new Cow(cowFormat: format, bubbleGenerator);

            var output = cow.Think("Hello world", cowEyes: "AB", cowTongue: "CD", maxCols: 10);

            output.ShouldBe(expectedOutput);
        }
    }
}
