using Cowsay;

var factory = new DefaultCattleFarmer(new EmbeddedCowFormatProvider(), new DefaultBubbleBlower());

var cow = await factory.RearCowAsync("default");

Console.WriteLine(cow.Say("Integer ullamcorper molestie nisi, in blandit sapien tempus non. Pellentesque pulvinar sed purus at ultrices. Quisque posuere ligula nec ante varius tristique. Integer sollicitudin porta pretium. Suspendisse dapibus eros metus, vel varius leo rutrum eu. Morbi eget eros ut urna varius porttitor. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer eu tincidunt arcu. Vivamus rhoncus ipsum sem, cursus mattis tortor molestie vitae. Nulla auctor augue vitae tellus vehicula, quis elementum neque tempor.", cowTongue: "U "));


Console.WriteLine(cow.Say("Hiya"));

Console.WriteLine(cow.Say("Environment".PadRight(20) + "UAT" + Environment.NewLine + "Service".PadRight(20) + "Holdings", cowEyes: "++"));

using (var stream = File.OpenRead("wealthify.cow"))
{
    var wealthiCow = await factory.RearCowFromFileStreamAsync(stream);
    Console.WriteLine(wealthiCow.Say("Wealthify ASCII art...", isThought: true));
}

var staticCow = await DefaultCattleFarmer.RearCowWithDefaults("bearface");

Console.WriteLine(staticCow.Say("I'm a static cow, no DI needed."));

Console.WriteLine(cow.Say("Short thought", isThought: true));
Console.WriteLine(cow.Say("Integer ullamcorper molestie nisi, in blandit sapien tempus non. Pellentesque pulvinar sed purus at ultrices. Quisque posuere ligula nec ante varius tristique.", isThought: true));
