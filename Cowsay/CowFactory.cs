using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cowsay
{
    public class CowFactory
    {
        public async Task<Cow> CreateCow(string cowName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            string cowFormat;

            var cows = assembly.GetManifestResourceNames()
                .Where(rn => rn.StartsWith("Cowsay.Cows"))
                .Select(rn => new { Name = Regex.Replace(rn, @"(^Cowsay\.Cows\.)*(\.cow$)*", string.Empty), FullPath = rn });

            if (!cows.Any(cow => cow.Name == cowName))
            {
                throw new FileNotFoundException($"{cowName}.cow embedded file not found");
            }
            else
            {
                cowFormat = ExtractCow(await ConvertToString(assembly.GetManifestResourceStream(cows.Single(c => c.Name == cowName).FullPath)));
            }

            return new Cow(cowFormat);
        }

        private Task<string> ConvertToString(Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            {
                return streamReader.ReadToEndAsync();
            }
        }

        private string ExtractCow(string cowString)
        {
            var match = RegularExpressions.Cow.Match(cowString);

            if (!match.Groups.Any(g => g.Name == "cow"))
            {
                throw new ArgumentException("Failed to extract cow from cow file.", nameof(cowString));
            }

            var cow = match.Groups["cow"].Value;

            cow = RegularExpressions.LineEndings.Replace(cow, Environment.NewLine)
                .Replace("\\\\", "\\");

            return cow;
        }
    }
}
