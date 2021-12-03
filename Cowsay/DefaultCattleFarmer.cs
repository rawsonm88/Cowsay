using Cowsay.Abstractions;
using System.IO;
using System.Threading.Tasks;

namespace Cowsay
{
    public class DefaultCattleFarmer : ICattleFarmer
    {
        private readonly ICowFormatProvider _cowFormatProvider;
        private readonly IBubbleBlower _bubbleBlower;

        public DefaultCattleFarmer(ICowFormatProvider cowFormatProvider, IBubbleBlower bubbleBlower)
        {
            _cowFormatProvider = cowFormatProvider;
            _bubbleBlower = bubbleBlower;
        }

        public static async Task<ICow> RearCowWithDefaults(string cowName)
        {
            var cowFormatProvider = new EmbeddedCowFormatProvider();
            var bubbleBlower = new DefaultBubbleBlower();

            var cowFormat = await cowFormatProvider.GetCowFormatAsync(cowName);

            return new Cow(cowFormat, bubbleBlower);
        }

        public async Task<ICow> RearCowAsync(string cowName)
        {
            string cowFormat = await _cowFormatProvider.GetCowFormatAsync(cowName);

            return new Cow(cowFormat, _bubbleBlower);
        }

        public async Task<ICow> RearCowFromFileStreamAsync(Stream cowStream)
        {
            var cowFile = new CowFile(await cowStream.ConvertToStringAsync(leaveOpen: true));
            return new Cow(await cowFile.GetCowFormatAsync(), _bubbleBlower);
        }
    }
}
