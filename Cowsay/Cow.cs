using Cowsay.Abstractions;
using System.Linq;

namespace Cowsay
{
    public class Cow : ICow
    {
        private readonly string _cowFormat;
        private readonly IBubbleBlower _bubbleGenerator;

        public Cow(string cowFormat, IBubbleBlower bubbleGenerator)
        {
            _cowFormat = cowFormat;
            _bubbleGenerator = bubbleGenerator;
        }

        public string Format => _cowFormat;

        public string Say(string phrase, string cowEyes = "oo", string cowTongue = "  ", int maxCols = 40, bool isThought = false)
        {
            cowTongue = cowTongue.PadRight(2);
            cowEyes = cowEyes.PadRight(2);

            string bubble = _bubbleGenerator.GetBubble(phrase, maxCols, isThought);

            string cow = _cowFormat
                .Replace("$eyes", cowEyes)
                .Replace("$tongue", cowTongue)
                .Replace("$thoughts", isThought ? "o": @"\");

            cow = RegularExpressions.Eye.Replace(cow, cowEyes.First().ToString(), 1);
            cow = RegularExpressions.Eye.Replace(cow, cowEyes.Last().ToString(), 1);

            return bubble + cow;
        }
    }
}