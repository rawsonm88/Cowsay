using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Cowsay
{
    public class Cow
    {
        private readonly string _cowFormat;

        public Cow(string cowFormat)
        {
            _cowFormat = cowFormat;
        }

        public string Say(string phrase, string cowEyes = "oo", string cowTongue = "  ", string cowThoughts = @"\", int maxCols = 40)
        {
            cowTongue = cowTongue.PadRight(2);
            cowEyes = cowEyes.PadRight(2);

            string bubble;

            if (phrase.Length > maxCols || phrase.Contains(Environment.NewLine))
            {
                bubble = GenerateMultiLineBubble(phrase, maxCols);
            }
            else
            {
                bubble = GenerateSingleLineBubble(phrase);
            }


            string cow = _cowFormat
                .Replace("$eyes", cowEyes)
                .Replace("$tongue", cowTongue)
                .Replace("$thoughts", cowThoughts);

            cow = RegularExpressions.Eye.Replace(cow, cowEyes.First().ToString(), 1);
            cow = RegularExpressions.Eye.Replace(cow, cowEyes.Last().ToString(), 1);

            return bubble + cow;
        }

        private static string GenerateSingleLineBubble(string phrase)
        {
            string bubble;
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(" " + string.Empty.PadRight(phrase.Length + 2, '_') + " ");
            stringBuilder.AppendLine("< " + phrase + " >");
            stringBuilder.Append(" " + string.Empty.PadRight(phrase.Length + 2, '-') + " ");

            bubble = stringBuilder.ToString();
            return bubble;
        }

        private string GenerateMultiLineBubble(string phrase, int maxCols)
        {
            int lineNumber = 0;
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(" " + string.Empty.PadRight(maxCols + 3, '_') + " ");

            string[] phrases = phrase.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToArray();
            int phraseNumber = 1;

            foreach (var inputLine in phrases)
            {
                string[] words = inputLine.Split(new[] { " " }, StringSplitOptions.None);

                string currentLine = string.Empty;

                foreach (var word in words)
                {
                    if (currentLine.Length + word.Length <= maxCols)
                    {
                        currentLine += (currentLine.Length > 0 ? " " : string.Empty) + word;
                    }
                    else
                    {
                        lineNumber++;

                        stringBuilder.AppendLine(GetPrefix(lineNumber) + " " + currentLine.PadRight(maxCols + 2) + GetSuffix(lineNumber));
                        currentLine = word;
                    }
                }

                if (phraseNumber == phrases.Length)
                {
                    stringBuilder.AppendLine("\\" + " " + currentLine.PadRight(maxCols + 2) + "/");
                }
                else
                {
                    lineNumber++;

                    stringBuilder.AppendLine(GetPrefix(lineNumber) + " " + currentLine.PadRight(maxCols + 2) + GetSuffix(lineNumber));
                }

                phraseNumber++;
            }

            stringBuilder.Append(" " + string.Empty.PadRight(maxCols + 3, '-') + " ");

            return stringBuilder.ToString();
        }

        private string GetPrefix(int lineNumber)
        {
            string prefix = "|";

            if (lineNumber == 1)
            {
                prefix = "/";
            }

            return prefix;
        }

        private string GetSuffix(int lineNumber)
        {
            string suffix = "|";

            if (lineNumber == 1)
            {
                suffix = "\\";
            }

            return suffix;
        }
    }
}