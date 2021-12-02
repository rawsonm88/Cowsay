using Cowsay.Abstractions;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cowsay
{
    public class EmbeddedCowFormatProvider : ICowFormatProvider
    {
        public async Task<string> GetCowFormatAsync(string cowName)
        {
            var assembly = typeof(DefaultCattleFarmer).Assembly;

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
                string cowFileContents;

                using (var stream = assembly.GetManifestResourceStream(cows.Single(c => c.Name == cowName).FullPath))
                {
                    cowFileContents = await stream.ConvertToStringAsync(leaveOpen: false);
                }

                var cowFile = new CowFile(cowFileContents);
                cowFormat = await cowFile.GetCowFormatAsync();
            }

            return cowFormat;
        }
    }
}
