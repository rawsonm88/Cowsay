using Cowsay.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cowsay
{
    public class DefaultBubbleBlower : IBubbleBlower
    {
        public string GetBubble(string phrase, int maxCols, bool isThoughtBubble)
        {
            string bubble;

            if (phrase.Length > maxCols || phrase.Contains(Environment.NewLine))
            {
                bubble = GenerateMultiLineBubble(phrase, maxCols, isThoughtBubble);
            }
            else
            {
                bubble = GenerateSingleLineBubble(phrase, isThoughtBubble);
            }

            return bubble;
        }

        private static string GenerateSingleLineBubble(string phrase, bool isThoughtBubble)
        {
            string bubble;
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(" " + string.Empty.PadRight(phrase.Length + 2, '_') + " ");
            stringBuilder.AppendLine((isThoughtBubble ? "( " : "< ") + phrase + (isThoughtBubble ? " )" : " >"));
            stringBuilder.AppendLine(" " + string.Empty.PadRight(phrase.Length + 2, '-') + " ");

            bubble = stringBuilder.ToString();
            return bubble;
        }

        private string GenerateMultiLineBubble(string phrase, int maxCols, bool isThoughtBubble)
        {
            StringBuilder stringBuilder = new StringBuilder();

            string[] linesWithoutDecoration = GetWrappedLines(phrase, maxCols);

            int maxWidth = linesWithoutDecoration.Max(l => l.Length);

            stringBuilder.AppendLine(" " + string.Empty.PadRight(maxWidth + 2, '_'));

            for (int i = 0; i < linesWithoutDecoration.Length; i++)
            {
                stringBuilder.AppendLine(
                    GetPrefix(i + 1, linesWithoutDecoration.Length, isThoughtBubble)
                    + " "
                    + linesWithoutDecoration[i].PadRight(maxWidth + 1)
                    + GetSuffix(i + 1, linesWithoutDecoration.Length, isThoughtBubble));
            }

            stringBuilder.AppendLine(" " + string.Empty.PadRight(maxWidth + 2, '-') + " ");

            return stringBuilder.ToString();
        }

        private string[] GetWrappedLines(string phrase, int maxCols)
        {
            int lineNumber = 0;

            string[] phrases = phrase.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToArray();

            List<string> wrappedLines = new List<string>();

            foreach (var inputLine in phrases)
            {
                string[] words = inputLine.Split(new[] { " " }, StringSplitOptions.None);

                string currentLine = string.Empty;

                foreach (var word in words)
                {
                    if (currentLine.Length + " ".Length + word.Length <= maxCols)
                    {
                        currentLine += (currentLine.Length > 0 ? " " : string.Empty) + word;
                    }
                    else
                    {
                        lineNumber++;

                        wrappedLines.Add(currentLine);
                        currentLine = word;
                    }
                }

                wrappedLines.Add(currentLine);
            }

            return wrappedLines.ToArray();
        }

        private string GetPrefix(int lineNumber, int lines, bool isThoughtBubble)
        {
            string prefix = "|";

            if (isThoughtBubble)
            {
                prefix = "(";
            }
            else if (lineNumber == 1)
            {
                prefix = "/";
            }
            else if (lineNumber == lines)
            {
                prefix = "\\";
            }

            return prefix;
        }

        private string GetSuffix(int lineNumber, int lines, bool thoughtBubble)
        {
            string suffix = "|";

            if (thoughtBubble)
            {
                suffix = ")";
            }
            else if (lineNumber == 1)
            {
                suffix = "\\";
            }
            else if (lineNumber == lines)
            {
                suffix = "/";
            }

            return suffix;
        }
    }
}
