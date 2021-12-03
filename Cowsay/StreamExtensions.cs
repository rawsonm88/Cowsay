using System.IO;
using System.Threading.Tasks;

namespace Cowsay
{
    internal static class StreamExtensions
    {
        internal static Task<string> ConvertToStringAsync(this Stream stream, bool leaveOpen = true)
        {
            stream.Seek(0, SeekOrigin.Begin);
            using (var streamReader = new StreamReader(stream, System.Text.Encoding.UTF8, false, 1024, leaveOpen: leaveOpen))
            {
                return streamReader.ReadToEndAsync();
            }
        }
    }
}
