using System;
using Cowsay.Abstractions;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Cowsay.UnitTests
{
    public class CowConstructorTests
    {
        [Fact]
        public void Constructor_throws_ArgumentNullException_for_null_cowFormat()
        {
            // Arrange
            var bubbleBlower = Substitute.For<IBubbleBlower>();

            // Act & Assert
            var exception = Should.Throw<ArgumentNullException>(() => new Cow(null, bubbleBlower));
            exception.ParamName.ShouldBe("cowFormat");
        }

        [Fact]
        public void Constructor_throws_ArgumentNullException_for_null_bubbleGenerator()
        {
            // Arrange
            const string cowFormat = "test format";

            // Act & Assert
            var exception = Should.Throw<ArgumentNullException>(() => new Cow(cowFormat, null));
            exception.ParamName.ShouldBe("bubbleGenerator");
        }

        [Fact]
        public void Constructor_accepts_empty_cowFormat()
        {
            // Arrange
            var bubbleBlower = Substitute.For<IBubbleBlower>();
            const string emptyFormat = "";

            // Act
            var cow = new Cow(emptyFormat, bubbleBlower);

            // Assert
            cow.ShouldNotBeNull();
            cow.Format.ShouldBe(emptyFormat);
        }

        [Fact]
        public void Format_property_returns_constructor_cowFormat()
        {
            // Arrange
            var bubbleBlower = Substitute.For<IBubbleBlower>();
            const string expectedFormat = "custom $eyes format $thoughts";

            // Act
            var cow = new Cow(expectedFormat, bubbleBlower);

            // Assert
            cow.Format.ShouldBe(expectedFormat);
        }
    }
}