using Cowsay.Abstractions;
using FluentAssertions;
using NSubstitute;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cowsay.UnitTests
{
    public class CowFactoryTests
    {
        [Fact]
        public async Task Create_from_provider_returns_expected_cow()
        {
            var provider = Substitute.For<ICowFormatProvider>();
            provider
                .GetCowFormatAsync("default")
                .Returns("abc$eyedef");

            var factory = new DefaultCattleFarmer(provider, null);

            var cow = await factory.RearCowAsync("default");

            cow.Format.Should().Be("abc$eyedef");
        }

        [Fact]
        public async Task Create_from_stream_returns_expected_cow()
        {
            using (var memoryStream = new MemoryStream(UTF8Encoding.UTF8.GetBytes("$the_cow = <<EOC;\r\nHello$eye$eye\r\nEOC")))
            {
                var factory = new DefaultCattleFarmer(null, null);

                var cow = await factory.RearCowFromFileStreamAsync(memoryStream);

                cow.Format.Should().Be("Hello$eye$eye");
            }
        }

        [Fact]
        public async Task Create_using_defaults_returns_expected_cow()
        {
            var cow = await DefaultCattleFarmer.RearCowWithDefaults("default");

            cow.Format.Should().Be(await File.ReadAllTextAsync(@"ExpectedOutputCows\default_cleaned.txt"));
        }
    }
}
