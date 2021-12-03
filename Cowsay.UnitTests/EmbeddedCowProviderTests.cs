using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System.IO;

namespace Cowsay.UnitTests
{
    public class EmbeddedCowProviderTests
    {
        [Fact]
        public async Task Non_existent_cow_throw_FileNotFoundException()
        {
            var provider = new EmbeddedCowFormatProvider();

            await provider.Invoking(p => p.GetCowFormatAsync("no-a-real-cow"))
                .Should().ThrowExactlyAsync<FileNotFoundException>();
        }

        [Fact]
        public async Task Real_cow_returns_cow_format_without_escaping()
        {
            var provider = new EmbeddedCowFormatProvider();

            var format = await provider.GetCowFormatAsync("default");

            format.Should().Be(await File.ReadAllTextAsync(@"ExpectedOutputCows\default_cleaned.txt"));
        }
    }
}
