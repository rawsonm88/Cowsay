using System.Threading.Tasks;
using Xunit;
using Shouldly;
using System.IO;
using System.Linq;

namespace Cowsay.UnitTests
{
    public class EmbeddedCowProviderTests
    {
        [Fact]
        public async Task Non_existent_cow_throw_FileNotFoundException()
        {
            var provider = new EmbeddedCowFormatProvider();

            var exception = await Should.ThrowAsync<FileNotFoundException>(
                async () => await provider.GetCowFormatAsync("no-a-real-cow"));
        }

        [Fact]
        public async Task Real_cow_returns_cow_format_without_escaping()
        {
            var provider = new EmbeddedCowFormatProvider();

            var format = await provider.GetCowFormatAsync("default");

            format.ShouldBe(await File.ReadAllTextAsync(Path.Combine("ExpectedOutputCows", "default_cleaned.txt")));
        }

        [Fact]
        public async Task GetAvailableCowsAsync_returns_all_embedded_cows()
        {
            var provider = new EmbeddedCowFormatProvider();

            var cows = await provider.GetAvailableCowsAsync();

            cows.ShouldNotBeNull();
            cows.ShouldNotBeEmpty();
            cows.ShouldContain("default");
            cows.ShouldContain("tux");
            cows.ShouldContain("stegosaurus");
            cows.ShouldContain("dragon");
            cows.ShouldContain("vader");
            cows.Count.ShouldBeGreaterThan(50);
        }

        [Fact]
        public async Task GetAvailableCowsAsync_returns_unique_cow_names()
        {
            var provider = new EmbeddedCowFormatProvider();

            var cows = await provider.GetAvailableCowsAsync();
            var cowList = cows.ToList();

            cowList.ShouldBeUnique();
        }

        [Fact]
        public async Task GetAvailableCowsAsync_returns_cow_names_without_extension()
        {
            var provider = new EmbeddedCowFormatProvider();

            var cows = await provider.GetAvailableCowsAsync();

            cows.ShouldAllBe(cow => !cow.EndsWith(".cow"));
            cows.ShouldAllBe(cow => !cow.Contains("Cowsay.Cows"));
        }
    }
}
