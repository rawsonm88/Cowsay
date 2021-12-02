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
            List<string> lines = new List<string>();

            foreach (var inputLine in phrase.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] words = inputLine.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                string currentLine = string.Empty;

                foreach (var word in words)
                {
                    if (currentLine.Length + word.Length <= maxCols)
                    {
                        currentLine += (currentLine.Length > 0 ? " " : string.Empty) + word;
                    }
                    else
                    {
                        lines.Add(currentLine.PadRight(maxCols + 2));
                        currentLine = word;
                    }
                }

                if (!string.IsNullOrWhiteSpace(currentLine))
                {
                    lines.Add(currentLine.PadRight(maxCols + 2));
                }
            }

            for (int lineNumber = 0; lineNumber < lines.Count; lineNumber++)
            {
                if (lineNumber == 0)
                {
                    lines[0] = "/ " + lines[0] + " \\";
                    continue;
                }

                if (lineNumber == lines.Count - 1)
                {
                    lines[lineNumber] = "\\ " + lines[lineNumber] + " /";
                    continue;
                }

                lines[lineNumber] = "| " + lines[lineNumber] + " |";
            }

            lines.Insert(0, " " + string.Empty.PadRight(maxCols + 3, '_') + " ");
            lines.Add(" " + string.Empty.PadRight(maxCols + 3, '-') + " ");

            return string.Join(Environment.NewLine, lines);
        }
    }
}