# cowsay

````
 __________________
< srsly dude, why? >
 ------------------
        \   ^__^
         \  (oo)\_______
            (__)\       )\/\
                ||----w |
                ||     ||
````

cowsay is a configurable talking cow, originally written in Perl by [Tony Monroe](https://github.com/tnalpgge/rank-amateur-cowsay)

This project is a translation in C#/.NET of the original program.

## Install

### With .NET DI
```
dotnet add package Cowsay
dotnet add package Cowsay.Extensions.DependencyInjection
```

### Without .NET DI
```
dotnet add package Cowsay
```

## Usage

### With .NET DI
```
public class MyClass
{
	private ICattleFarmer _cattleFarmer;

	public MyClass(ICattleFarmer cattleFarmer)
	{
		_cattleFarmer = cattleFarmer;
	}

	public async Task DoThing()
	{
		var myCow = await cattleFarmer.RearCowAsync("bearface");

		Console.WriteLine(myCow.Say("I was reared on dependency injection.");
	}
}
```

#### Output
```
 _______________________________________
< I was reared on dependency injection. >
 ---------------------------------------
 \
  \
     .--.              .--.
    : (\ ". _......_ ." /) :
     '.    `        `    .'
      /'   _        _   `\
     /     o}      {o     \
    |       /      \       |
    |     /'        `\     |
     \   | .  .==.  . |   /
      '._ \.' \__/ './ _.'
      /  ``'._-''-_.'``  \
```

### Without .NET DI
```
var staticCow = await DefaultCattleFarmer.RearCowWithDefaults("default");

Console.WriteLine(staticCow.Say("I'm a static cow, no DI needed."));
```

#### Output
```
 _________________________________
< I'm a static cow, no DI needed. >
 ---------------------------------
        \   ^__^
         \  (oo)\_______
            (__)\       )\/\
                ||----w |
                ||     ||
```