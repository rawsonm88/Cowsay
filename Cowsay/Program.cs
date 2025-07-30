using System;
using System.Threading.Tasks;

namespace Cowsay
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var message = (args.Length > 0 && !args[0].StartsWith("--")) ? args[0] : "Hello from the command line!";
            var cow = Array.Find(args, a => a.StartsWith("--cow="))?.Split('=')[1] ?? "default";
            var eyes = Array.Find(args, a => a.StartsWith("--eyes="))?.Split('=')[1];
            var tongue = Array.Find(args, a => a.StartsWith("--tongue="))?.Split('=')[1];

            var cattleFarmer = new DefaultCattleFarmer(new EmbeddedCowFormatProvider(), new DefaultBubbleBlower());
            var talkingCow = await cattleFarmer.RearCowAsync(cow);

            Console.WriteLine(talkingCow.Say(message, eyes ?? "oo", tongue ?? "  "));
        }
    }
}
