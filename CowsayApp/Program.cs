using Cowsay;

var factory = new CowFactory();

var cow = await factory.CreateCow("default");

Console.WriteLine(cow.Say("Integer ullamcorper molestie nisi, in blandit sapien tempus non. Pellentesque pulvinar sed purus at ultrices. Quisque posuere ligula nec ante varius tristique. Integer sollicitudin porta pretium. Suspendisse dapibus eros metus, vel varius leo rutrum eu. Morbi eget eros ut urna varius porttitor. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer eu tincidunt arcu. Vivamus rhoncus ipsum sem, cursus mattis tortor molestie vitae. Nulla auctor augue vitae tellus vehicula, quis elementum neque tempor.", cowTongue: "U "));


Console.WriteLine(cow.Say("Hiya"));

Console.WriteLine(cow.Say("Environment".PadRight(20) + "UAT" + Environment.NewLine + "Service".PadRight(20) + "Holdings", cowEyes: "++"));
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
