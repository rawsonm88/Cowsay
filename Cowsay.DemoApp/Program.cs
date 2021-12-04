using Cowsay;
using Cowsay.Abstractions;
using Microsoft.Extensions.DependencyInjection;

var factory = new DefaultCattleFarmer(new EmbeddedCowFormatProvider(), new DefaultBubbleBlower());

var cow = await factory.RearCowAsync("default");

Console.WriteLine(cow.Say("Integer ullamcorper molestie nisi, in blandit sapien tempus non. Pellentesque pulvinar sed purus at ultrices. Quisque posuere ligula nec ante varius tristique. Integer sollicitudin porta pretium. Suspendisse dapibus eros metus, vel varius leo rutrum eu. Morbi eget eros ut urna varius porttitor. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer eu tincidunt arcu. Vivamus rhoncus ipsum sem, cursus mattis tortor molestie vitae. Nulla auctor augue vitae tellus vehicula, quis elementum neque tempor.", cowTongue: "U "));


Console.WriteLine(cow.Say("Hiya"));

Console.WriteLine(cow.Say("Environment".PadRight(20) + "Production"
    + Environment.NewLine + "Service".PadRight(20) + "Order"
    + Environment.NewLine + "Node".PadRight(20) + "lin-1234562", cowEyes: "++"));

using (var stream = File.OpenRead("wealthify.cow"))
{
    var wealthiCow = await factory.RearCowFromFileStreamAsync(stream);
    Console.WriteLine(wealthiCow.Say("Wealthify ASCII art...", isThought: true));
}

var staticCow = await DefaultCattleFarmer.RearCowWithDefaults("default");

Console.WriteLine(staticCow.Say("I'm a static cow, no DI needed."));

Console.WriteLine(cow.Say("Short thought", isThought: true));
Console.WriteLine(cow.Say("Integer ullamcorper molestie nisi, in blandit sapien tempus non. Pellentesque pulvinar sed purus at ultrices. Quisque posuere ligula nec ante varius tristique.", isThought: true));

IServiceCollection services = new ServiceCollection();
services.AddCowsay();
var provider = services.BuildServiceProvider();

var farmer = provider.GetRequiredService<ICattleFarmer>();

var cowFromDI = await farmer.RearCowAsync("bearface");
Console.WriteLine(cowFromDI.Say("I was reared on dependency injection."));