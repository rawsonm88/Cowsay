using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Cowsay.Abstractions;

namespace Cowsay
{
    public class EmbeddedCowFormatProvider : ICowFormatProvider
    {
        private readonly Lazy<IReadOnlyList<(string Name, string FullPath)>> _cachedCows;
        private const string ResourcePrefix = "Cowsay.Cows";
        private static readonly Regex CowNamePattern = new Regex(@"(^Cowsay\.Cows\.)*(\.cow$)*", RegexOptions.Compiled);

        public EmbeddedCowFormatProvider()
        {
            _cachedCows = new Lazy<IReadOnlyList<(string Name, string FullPath)>>(
                LoadCows,
                LazyThreadSafetyMode.PublicationOnly);
        }

        public async Task<string> GetCowFormatAsync(string cowName)
        {
            var cows = _cachedCows.Value;
            var cow = cows.FirstOrDefault(c => c.Name == cowName);

            if (cow.FullPath == null)
            {
                throw new FileNotFoundException($"{cowName}.cow embedded file not found");
            }

            string cowFileContents;
            var assembly = typeof(DefaultCattleFarmer).Assembly;
            using (var stream = assembly.GetManifestResourceStream(cow.FullPath))
            {
                cowFileContents = await stream.ConvertToStringAsync(leaveOpen: false).ConfigureAwait(false);
            }

            var cowFile = new CowFile(cowFileContents);
            return await cowFile.GetCowFormatAsync().ConfigureAwait(false);
        }

        public Task<IReadOnlyList<string>> GetAvailableCowsAsync()
        {
            var cows = _cachedCows.Value;
            var names = cows.Select(c => c.Name).ToList().AsReadOnly();
            return Task.FromResult<IReadOnlyList<string>>(names);
        }

        private static IReadOnlyList<(string Name, string FullPath)> LoadCows()
        {
            var assembly = typeof(DefaultCattleFarmer).Assembly;
            return assembly.GetManifestResourceNames()
                .Where(rn => rn.StartsWith(ResourcePrefix))
                .Select(rn => (Name: CowNamePattern.Replace(rn, string.Empty), FullPath: rn))
                .OrderBy(c => c.Name)
                .ToList()
                .AsReadOnly();
        }
    }
}
