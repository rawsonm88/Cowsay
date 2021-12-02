using FluentAssertions;
using System;
using Xunit;

namespace Cowsay.UnitTests
{
    public class DefaultBubbleBlowerTests
    {
        [Fact]
        public void Single_line_bubble_generated_correctly()
        {
            var bubbleGenerator = new DefaultBubbleBlower();

            var bubble = bubbleGenerator.GetBubble("Hello", 40, isThoughtBubble: false);

            bubble.Should().Be($" _______ {Environment.NewLine}< Hello >{Environment.NewLine} ------- {Environment.NewLine}");
        }

        [Fact]
        public void Multi_line_with_single_phrase_bubble_generated_correctly()
        {
            var bubbleGenerator = new DefaultBubbleBlower();

            var bubble = bubbleGenerator.GetBubble("Hello world, this is a bubble", 7, isThoughtBubble: false);

            bubble.Should().Be($" _________{Environment.NewLine}/ Hello   \\{Environment.NewLine}| world,  |{Environment.NewLine}| this is |{Environment.NewLine}| a       |{Environment.NewLine}\\ bubble  /{Environment.NewLine} --------- {Environment.NewLine}");
        }

        [Fact]
        public void Multi_line_with_multiple_phrases_bubble_generated_correctly()
        {
            var bubbleGenerator = new DefaultBubbleBlower();

            var bubble = bubbleGenerator.GetBubble($"Hello world{Environment.NewLine}This{Environment.NewLine}Cow{Environment.NewLine}This should be multiple lines", 11, isThoughtBubble: false);

            bubble.Should().Be($" _____________{Environment.NewLine}/ Hello world \\{Environment.NewLine}| This        |{Environment.NewLine}| Cow         |{Environment.NewLine}| This should |{Environment.NewLine}| be multiple |{Environment.NewLine}\\ lines       /{Environment.NewLine} ------------- {Environment.NewLine}");
        }

        [Fact]
        public void Multi_line_thought_bubble_generated_correctly()
        {
            var bubbleGenerator = new DefaultBubbleBlower();

            var bubble = bubbleGenerator.GetBubble("Hello world, this is a bubble", 7, isThoughtBubble: true);

            bubble.Should().Be($" _________{Environment.NewLine}( Hello   ){Environment.NewLine}( world,  ){Environment.NewLine}( this is ){Environment.NewLine}( a       ){Environment.NewLine}( bubble  ){Environment.NewLine} --------- {Environment.NewLine}");
        }

        [Fact]
        public void Single_line_thought_bubble_generated_correctly()
        {
            var bubbleGenerator = new DefaultBubbleBlower();

            var bubble = bubbleGenerator.GetBubble("Hello", 40, isThoughtBubble: true);

            bubble.Should().Be($" _______ {Environment.NewLine}( Hello ){Environment.NewLine} ------- {Environment.NewLine}");
        }
    }
}
