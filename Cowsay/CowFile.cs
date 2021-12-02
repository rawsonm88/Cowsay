using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Cowsay
{
    internal class CowFile
    {
        private readonly Stream _fileStream;
        private string _fileContents;
        private bool _stringLoaded = false;
        private SemaphoreSlim _streamLock = new SemaphoreSlim(1);

        internal CowFile(Stream fileStream)
        {
            _fileStream = fileStream;
        }

        internal CowFile(string fileContents)
        {
            _fileContents = fileContents;
            _stringLoaded = true;
        }

        internal async Task<string> GetCowFormatAsync()
        {
            if (!_stringLoaded)
            {
                await _streamLock.WaitAsync();

                try
                {
                    if (!_stringLoaded)
                    {
                        _fileContents = await _fileStream.ConvertToStringAsync(leaveOpen: true);
                        _stringLoaded = true;
                    }
                }
                finally
                {
                    _streamLock.Release();
                }
            }

            string cowFormat = ExtractCow(_fileContents);
            return cowFormat;
        }

        private string ExtractCow(string cowString)
        {
            var match = RegularExpressions.Cow.Match(cowString);

            if (!match.Groups["cow"].Success)
            {
                throw new ArgumentException("Failed to extract cow from cow file.", nameof(cowString));
            }

            var cow = match.Groups["cow"].Value;

            cow = RegularExpressions.LineEndings.Replace(cow, Environment.NewLine)
                .Replace("\\\\", "\\");

            return cow;
        }

        private enum LoadMode
        {
            Stream,
            String
        }
    }
}
