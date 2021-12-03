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

            bubble.Should().Be(" _______ \r\n< Hello >\r\n ------- \r\n");
        }

        [Fact]
        public void Multi_line_with_single_phrase_bubble_generated_correctly()
        {
            var bubbleGenerator = new DefaultBubbleBlower();

            var bubble = bubbleGenerator.GetBubble("Hello world, this is a bubble", 7, isThoughtBubble: false);

            bubble.Should().Be(" _________\r\n/ Hello   \\\r\n| world,  |\r\n| this is |\r\n| a       |\r\n\\ bubble  /\r\n --------- \r\n");
        }

        [Fact]
        public void Multi_line_with_multiple_phrases_bubble_generated_correctly()
        {
            var bubbleGenerator = new DefaultBubbleBlower();

            var bubble = bubbleGenerator.GetBubble($"Hello world{Environment.NewLine}This{Environment.NewLine}Cow{Environment.NewLine}This should be multiple lines", 11, isThoughtBubble: false);

            bubble.Should().Be(" _____________\r\n/ Hello world \\\r\n| This        |\r\n| Cow         |\r\n| This should |\r\n| be multiple |\r\n\\ lines       /\r\n ------------- \r\n");
        }

        [Fact]
        public void Multi_line_thought_bubble_generated_correctly()
        {
            var bubbleGenerator = new DefaultBubbleBlower();

            var bubble = bubbleGenerator.GetBubble("Hello world, this is a bubble", 7, isThoughtBubble: true);

            bubble.Should().Be(" _________\r\n( Hello   )\r\n( world,  )\r\n( this is )\r\n( a       )\r\n( bubble  )\r\n --------- \r\n");
        }

        [Fact]
        public void Single_line_thought_bubble_generated_correctly()
        {
            var bubbleGenerator = new DefaultBubbleBlower();

            var bubble = bubbleGenerator.GetBubble("Hello", 40, isThoughtBubble: true);

            bubble.Should().Be(" _______ \r\n( Hello )\r\n ------- \r\n");
        }
    }
}
