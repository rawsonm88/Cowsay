# cowsay (.NET)

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

[![.NET Build](https://github.com/rawsonm88/Cowsay/actions/workflows/build.yml/badge.svg?branch=master)](https://github.com/rawsonm88/Cowsay/actions/workflows/build.yml) [![Coverage Status](https://coveralls.io/repos/github/rawsonm88/Cowsay/badge.svg?branch=master)](https://coveralls.io/github/rawsonm88/Cowsay?branch=master)

cowsay is a configurable talking cow, originally written in Perl by [Tony Monroe](https://github.com/tnalpgge/rank-amateur-cowsay)

This project is a translation in C#/.NET of the original program. It includes both a library for easy integration into your own projects (for example a startup splashscreen for a CLI app) and a standalone CLI tool.

```
 _________________________________
/ Environment         Production  \
| Service             Order       |
\ Node                lin-1234562 /
 ---------------------------------
        \   ^__^
         \  (++)\_______
            (__)\       )\/\
                ||----w |
                ||     ||
```

The [`.cow` files](Cowsay/Cows) were manually copied from https://github.com/piuccio/cowsay.

## Install

### CLI Tool
```
dotnet tool install -g Cowsay.CLI
```

See the [CLI documentation](src/Cowsay.CLI/README.md) for usage instructions.

### Library - With .NET DI
```
dotnet add package Cowsay
dotnet add package Cowsay.Extensions.DependencyInjection
```

### Library - Without .NET DI
```
dotnet add package Cowsay
```

## Library Usage

### With .NET DI
```C#
services.AddCowsay();
```

```C#
public class MyClass
{
    private ICattleFarmer _cattleFarmer;

    public MyClass(ICattleFarmer cattleFarmer)
    {
       _cattleFarmer = cattleFarmer;
    }

    public async Task DoThing()
    {
       var myCow = await cattleFarmer.RearCowAsync("default");

       Console.WriteLine(myCow.Say("I was reared on dependency injection.");
    }
}
```

#### Output
```
 _______________________________________
< I was reared on dependency injection. >
 ---------------------------------------
        \   ^__^
         \  (oo)\_______
            (__)\       )\/\
                ||----w |
                ||     ||
```

### Without .NET DI
```C#
var staticCow = await DefaultCattleFarmer.RearCowWithDefaults("default");

Console.WriteLine(staticCow.Say("I'm a static cow, no DI needed.", cowEyes: "xx"));
```

#### Output
```
 _________________________________
< I'm a static cow, no DI needed. >
 ---------------------------------
        \   ^__^
         \  (xx)\_______
            (__)\       )\/\
                ||----w |
                ||     ||
```
