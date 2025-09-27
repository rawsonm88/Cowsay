using System.IO;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Cowsay.UnitTests
{
    public class StreamExtensionsTests
    {
        [Fact]
        public async Task ConvertToStringAsync_reads_stream_content()
        {
            // Arrange
            const string expectedContent = "Hello, World!";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(expectedContent));

            // Act
            var result = await stream.ConvertToStringAsync();

            // Assert
            result.ShouldBe(expectedContent);
        }

        [Fact]
        public async Task ConvertToStringAsync_with_leaveOpen_true_keeps_stream_open()
        {
            // Arrange
            const string expectedContent = "Test content";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(expectedContent));

            // Act
            var result = await stream.ConvertToStringAsync(leaveOpen: true);

            // Assert
            result.ShouldBe(expectedContent);
            stream.CanRead.ShouldBeTrue(); // Stream should still be open
        }

        [Fact]
        public async Task ConvertToStringAsync_with_leaveOpen_false_closes_stream()
        {
            // Arrange
            const string expectedContent = "Test content";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(expectedContent));

            // Act
            var result = await stream.ConvertToStringAsync(leaveOpen: false);

            // Assert
            result.ShouldBe(expectedContent);
            stream.CanRead.ShouldBeFalse(); // Stream should be closed
        }

        [Fact]
        public async Task ConvertToStringAsync_resets_stream_position_before_reading()
        {
            // Arrange
            const string expectedContent = "Reset position test";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(expectedContent));

            // Move stream position to middle
            stream.Seek(5, SeekOrigin.Begin);

            // Act
            var result = await stream.ConvertToStringAsync();

            // Assert
            result.ShouldBe(expectedContent); // Should read from beginning despite initial position
        }

        [Fact]
        public async Task ConvertToStringAsync_handles_empty_stream()
        {
            // Arrange
            using var stream = new MemoryStream();

            // Act
            var result = await stream.ConvertToStringAsync();

            // Assert
            result.ShouldBe(string.Empty);
        }

        [Fact]
        public async Task ConvertToStringAsync_handles_unicode_content()
        {
            // Arrange
            const string expectedContent = "Hello 世界 🐮";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(expectedContent));

            // Act
            var result = await stream.ConvertToStringAsync();

            // Assert
            result.ShouldBe(expectedContent);
        }

        [Fact]
        public async Task ConvertToStringAsync_handles_multiline_content()
        {
            // Arrange
            const string expectedContent = "Line 1\nLine 2\r\nLine 3";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(expectedContent));

            // Act
            var result = await stream.ConvertToStringAsync();

            // Assert
            result.ShouldBe(expectedContent);
        }

        [Fact]
        public async Task ConvertToStringAsync_can_be_called_multiple_times_with_leaveOpen()
        {
            // Arrange
            const string expectedContent = "Reusable content";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(expectedContent));

            // Act
            var result1 = await stream.ConvertToStringAsync(leaveOpen: true);
            var result2 = await stream.ConvertToStringAsync(leaveOpen: true);

            // Assert
            result1.ShouldBe(expectedContent);
            result2.ShouldBe(expectedContent);
        }
    }
}