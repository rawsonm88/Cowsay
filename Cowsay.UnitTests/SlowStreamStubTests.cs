using Cowsay.UnitTests.Stubs;
using FluentAssertions;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Cowsay.UnitTests
{
    public class SlowStreamStubTests
    {
        [Fact]
        public async Task Ten_tasks_accessing_stream_simultaneously()
        {
            int threads = 10;

            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream))
            {
                await streamWriter.WriteLineAsync("Hello world");
                await streamWriter.FlushAsync();

                using (var slowStream = new SlowStream(memoryStream))
                {
                    for (int i = 0; i < threads; i++)
                    {
                        _ = Task.Run(async () => await slowStream.ConvertToStringAsync());
                    }

                    await Task.Delay(100);

                    slowStream.ThreadsReadingCount.Should().Be(threads);
                }
            }
        }
    }
}
