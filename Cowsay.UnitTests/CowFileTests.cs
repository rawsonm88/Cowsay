using System;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System.IO;
using Cowsay.UnitTests.Stubs;

namespace Cowsay.UnitTests
{
    public class CowFileTests
    {
        [Fact]
        public async Task Successfully_loads_cow_from_string()
        {
            var cowFile = new CowFile(File.ReadAllText(@"TestCows\default.cow"));

            var format = await cowFile.GetCowFormatAsync();

            format.Should().Be(File.ReadAllText(@"ExpectedOutputCows\default_cleaned.txt"));
        }

        [Fact]
        public async Task Successfully_loads_cow_from_stream()
        {
            string format;

            using (var stream = File.OpenRead(@"TestCows\default.cow"))
            {
                var cowFile = new CowFile(stream);
                format = await cowFile.GetCowFormatAsync();
            }

            format.Should().Be(File.ReadAllText(@"ExpectedOutputCows\default_cleaned.txt"));
        }

        [Fact]
        public async Task Throws_ArgumentException_if_cow_cant_be_parsed()
        {
            var cowFile = new CowFile("not a cow");

            await cowFile.Invoking(c => c.GetCowFormatAsync())
                .Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task Multiple_threads_accessing_cowfile_reads_stream_once()
        {
            string expectedFormat = File.ReadAllText(@"ExpectedOutputCows\default_cleaned.txt");

            using (var stream = File.OpenRead(@"TestCows\default.cow"))
            using (var slowStream = new SlowStream(stream))
            {
                var cowFile = new CowFile(slowStream);
                Task<string>[] tasks = new Task<string>[10];

                for (int i = 0; i < 10; i++)
                {
                    tasks[i] = Task.Run(async () => await cowFile.GetCowFormatAsync());
                }

                await Task.Delay(100);

                slowStream.ThreadsReadingCount.Should().Be(1);

                await Task.WhenAll(tasks);

                tasks.Should().OnlyContain(f => f.Result == expectedFormat);
            }
        }
    }
}
