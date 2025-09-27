using Cowsay.Abstractions;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Cowsay.UnitTests
{
    public class CowEdgeCaseTests
    {
        [Fact]
        public void Speak_pads_short_eyes_to_two_characters()
        {
            // Arrange
            var bubbleBlower = Substitute.For<IBubbleBlower>();
            bubbleBlower.GetBubble(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<bool>()).Returns("bubble");
            var cow = new Cow("$eyes", bubbleBlower);

            // Act
            var result = cow.Speak("test", cowEyes: "x", cowTongue: "  ");

            // Assert
            result.ShouldContain("x "); // Should be padded to 2 chars
        }

        [Fact]
        public void Speak_pads_short_tongue_to_two_characters()
        {
            // Arrange
            var bubbleBlower = Substitute.For<IBubbleBlower>();
            bubbleBlower.GetBubble(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<bool>()).Returns("bubble");
            var cow = new Cow("$tongue", bubbleBlower);

            // Act
            var result = cow.Speak("test", cowEyes: "oo", cowTongue: "U");

            // Assert
            result.ShouldContain("U "); // Should be padded to 2 chars
        }

        [Fact]
        public void Speak_handles_empty_eyes()
        {
            // Arrange
            var bubbleBlower = Substitute.For<IBubbleBlower>();
            bubbleBlower.GetBubble(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<bool>()).Returns("bubble");
            var cow = new Cow("$eyes", bubbleBlower);

            // Act
            var result = cow.Speak("test", cowEyes: "", cowTongue: "  ");

            // Assert
            result.ShouldContain("  "); // Should be padded to 2 spaces
        }

        [Fact]
        public void Speak_handles_empty_tongue()
        {
            // Arrange
            var bubbleBlower = Substitute.For<IBubbleBlower>();
            bubbleBlower.GetBubble(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<bool>()).Returns("bubble");
            var cow = new Cow("$tongue", bubbleBlower);

            // Act
            var result = cow.Speak("test", cowEyes: "oo", cowTongue: "");

            // Assert
            result.ShouldContain("  "); // Should be padded to 2 spaces
        }

        [Fact]
        public void Speak_truncates_long_eyes_to_first_and_last_character()
        {
            // Arrange
            var bubbleBlower = Substitute.For<IBubbleBlower>();
            bubbleBlower.GetBubble(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<bool>()).Returns("bubble");
            var cow = new Cow("$eye $eye", bubbleBlower);

            // Act
            var result = cow.Speak("test", cowEyes: "ABCDEFG", cowTongue: "  ");

            // Assert
            result.ShouldContain("A ");
            result.ShouldContain(" G");
        }

        [Fact]
        public void Think_sets_thoughts_to_o()
        {
            // Arrange
            var bubbleBlower = Substitute.For<IBubbleBlower>();
            bubbleBlower.GetBubble(Arg.Any<string>(), Arg.Any<int>(), true).Returns("bubble");
            var cow = new Cow("$thoughts", bubbleBlower);

            // Act
            var result = cow.Think("test");

            // Assert
            result.ShouldContain("o");
        }

        [Fact]
        public void Speak_sets_thoughts_to_backslash()
        {
            // Arrange
            var bubbleBlower = Substitute.For<IBubbleBlower>();
            bubbleBlower.GetBubble(Arg.Any<string>(), Arg.Any<int>(), false).Returns("bubble");
            var cow = new Cow("$thoughts", bubbleBlower);

            // Act
            var result = cow.Speak("test");

            // Assert
            result.ShouldContain(@"\");
        }

        [Fact]
        public void Cow_handles_format_without_placeholders()
        {
            // Arrange
            var bubbleBlower = Substitute.For<IBubbleBlower>();
            bubbleBlower.GetBubble(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<bool>()).Returns("bubble");
            var cow = new Cow("just a plain cow", bubbleBlower);

            // Act
            var result = cow.Speak("test");

            // Assert
            result.ShouldBe("bubblejust a plain cow");
        }

        [Fact]
        public void Cow_preserves_format_with_unknown_placeholders()
        {
            // Arrange
            var bubbleBlower = Substitute.For<IBubbleBlower>();
            bubbleBlower.GetBubble(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<bool>()).Returns("bubble");
            var cow = new Cow("$unknown $placeholder", bubbleBlower);

            // Act
            var result = cow.Speak("test");

            // Assert
            result.ShouldBe("bubble$unknown $placeholder");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(int.MaxValue)]
        public void Speak_passes_maxCols_to_bubble_generator(int maxCols)
        {
            // Arrange
            var bubbleBlower = Substitute.For<IBubbleBlower>();
            var cow = new Cow("test", bubbleBlower);

            // Act
            cow.Speak("test", maxCols: maxCols);

            // Assert
            bubbleBlower.Received(1).GetBubble("test", maxCols, false);
        }

        [Fact]
        public void Say_with_isThought_false_calls_Speak()
        {
            // Arrange
            var bubbleBlower = Substitute.For<IBubbleBlower>();
            bubbleBlower.GetBubble(Arg.Any<string>(), Arg.Any<int>(), false).Returns("speech");
            var cow = new Cow("cow", bubbleBlower);

            // Act
#pragma warning disable CS0618
            var result = cow.Say("test", isThought: false);
#pragma warning restore CS0618

            // Assert
            bubbleBlower.Received(1).GetBubble("test", 40, false);
            result.ShouldContain("speech");
        }

        [Fact]
        public void Say_with_isThought_true_calls_Think()
        {
            // Arrange
            var bubbleBlower = Substitute.For<IBubbleBlower>();
            bubbleBlower.GetBubble(Arg.Any<string>(), Arg.Any<int>(), true).Returns("thought");
            var cow = new Cow("cow", bubbleBlower);

            // Act
#pragma warning disable CS0618
            var result = cow.Say("test", isThought: true);
#pragma warning restore CS0618

            // Assert
            bubbleBlower.Received(1).GetBubble("test", 40, true);
            result.ShouldContain("thought");
        }
    }
}