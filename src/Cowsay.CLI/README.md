# Cowsay CLI (.NET Tool)

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

A .NET global tool that brings the classic cowsay to your terminal.

## About

cowsay is a configurable talking cow, originally written in Perl by [Tony Monroe](https://github.com/tnalpgge/rank-amateur-cowsay).

This CLI tool is part of the Cowsay .NET project - a C#/.NET translation of the original program. The [`.cow` files](../Cowsay/Cows) were manually copied from https://github.com/piuccio/cowsay.

## Installation

### From NuGet
```bash
dotnet tool install --global Cowsay.CLI
```

### Using dnx (.NET 10+)
With .NET 10 or later, you can run the tool without installing it:
```bash
dnx Cowsay.CLI "Hello, World!"
```

## Usage

### Basic Usage
```bash
cowsay "Hello, World!"
```

````
 ______________
< Hello, World! >
 --------------
        \   ^__^
         \  (oo)\_______
            (__)\       )\/\
                ||----w |
                ||     ||
````

### Piped Input
```bash
echo "Hello from pipe" | cowsay
fortune | cowsay
```

## Options

- `-l, --list` - List all available cow formats
- `-c, --cow <name>` - Choose a cow format (default: "default")
- `-e, --eyes <chars>` - Set the cow's eyes (2 characters, e.g., '@@', 'xx', '$$')
- `-t, --tongue <chars>` - Set the cow's tongue (2 characters, e.g., 'U ', '~ ')
- `-w, --wrap <cols>` - Wrap text at N columns (default: 40)
- `-T, --think` - Make the cow think instead of say
- `--help` - Display help with examples
- `--version` - Display version information

## Examples

### Custom Eyes
```bash
cowsay -e @@ "I see you"
```
````
 ___________
< I see you >
 -----------
        \   ^__^
         \  (@@)\_______
            (__)\       )\/\
                ||----w |
                ||     ||
````

### Thinking Cow
```bash
cowsay -T "Hmm..."
```
````
 ________
( Hmm... )
 --------
        o   ^__^
         o  (oo)\_______
            (__)\       )\/\
                ||----w |
                ||     ||
````

### Different Cow Format
```bash
cowsay -c tux "Linux rocks!"
```
````
 ______________ 
< Linux rocks! >
 -------------- 
   \
    \
        .--.
       |o_o |
       |:_/ |
      //   \ \
     (|     | )
    /'\_   _/`\
    \___)=(___/
````

### List Available Cows
```bash
cowsay -l
```

## Building from Source

```bash
# Clone the repository
git clone https://github.com/rawsonm88/Cowsay.git
cd Cowsay/Cowsay.CLI

# Build and pack
dotnet build
dotnet pack

# Install from local package
dotnet tool install --global --add-source ./nupkg Cowsay.CLI
```

## Uninstalling

```bash
dotnet tool uninstall --global Cowsay.CLI
```

## Related Packages

This CLI tool uses the Cowsay .NET library. If you want to use cowsay in your .NET applications:

- **Library Package**: `Cowsay` - Core library
- **DI Package**: `Cowsay.Extensions.DependencyInjection` - Dependency injection support

See the [main project README](../README.md) for library usage.

## Credits

- Original Perl cowsay by [Tony Monroe](https://github.com/tnalpgge/rank-amateur-cowsay)
- .cow files from [piuccio/cowsay](https://github.com/piuccio/cowsay)