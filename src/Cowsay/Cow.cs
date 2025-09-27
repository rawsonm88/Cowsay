using System;
using System.Text;
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
            _cowFormat = cowFormat ?? throw new ArgumentNullException(nameof(cowFormat));
            _bubbleGenerator = bubbleGenerator ?? throw new ArgumentNullException(nameof(bubbleGenerator));
        }

        public string Format => _cowFormat;

        public string Speak(string phrase, string cowEyes = "oo", string cowTongue = "  ", int maxCols = 40)
        {
            return Act(phrase, cowEyes, cowTongue, maxCols, isThought: false);
        }

        [Obsolete("Use Speak for speech or Think for thoughts")]
        public string Say(string phrase, string cowEyes = "oo", string cowTongue = "  ", int maxCols = 40, bool isThought = false)
        {
            if (isThought)
                return Think(phrase, cowEyes, cowTongue, maxCols);

            return Speak(phrase, cowEyes, cowTongue, maxCols);
        }

        public string Think(string phrase, string cowEyes = "oo", string cowTongue = "  ", int maxCols = 40)
        {
            return Act(phrase, cowEyes, cowTongue, maxCols, isThought: true);
        }

        private string Act(string phrase, string cowEyes = "oo", string cowTongue = "  ", int maxCols = 40, bool isThought = false)
        {
            cowTongue = cowTongue.PadRight(2);
            cowEyes = cowEyes.PadRight(2);

            string bubble = _bubbleGenerator.GetBubble(phrase, maxCols, isThought);

            var cowBuilder = new StringBuilder(_cowFormat);
            cowBuilder.Replace("$eyes", cowEyes);
            cowBuilder.Replace("$tongue", cowTongue);
            cowBuilder.Replace("$thoughts", isThought ? "o": @"\");

            string cow = cowBuilder.ToString();
            cow = RegularExpressions.Eye.Replace(cow, cowEyes.First().ToString(), 1);
            cow = RegularExpressions.Eye.Replace(cow, cowEyes.Last().ToString(), 1);

            return bubble + cow;
        }
    }
}