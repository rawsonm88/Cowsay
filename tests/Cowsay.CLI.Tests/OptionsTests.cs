using Cowsay.CLI.Infrastructure;
using Shouldly;
using Xunit;

namespace Cowsay.CLI.Tests;

public class OptionsTests
{
    [Fact]
    public void Options_DefaultValues_AreSetCorrectly()
    {
        var options = new Options();

        options.Message.ShouldBe(string.Empty);
        options.List.ShouldBeFalse();
        options.Cow.ShouldBe("default");
        options.File.ShouldBeNull();
        options.Eyes.ShouldBeNull();
        options.Tongue.ShouldBeNull();
        options.Wrap.ShouldBeNull();
        options.Think.ShouldBeFalse();
    }

    [Fact]
    public void Options_MessageProperty_CanBeSet()
    {
        var options = new Options { Message = "Test message" };

        options.Message.ShouldBe("Test message");
    }

    [Fact]
    public void Options_ListProperty_CanBeSet()
    {
        var options = new Options { List = true };

        options.List.ShouldBeTrue();
    }

    [Fact]
    public void Options_CowProperty_CanBeSet()
    {
        var options = new Options { Cow = "tux" };

        options.Cow.ShouldBe("tux");
    }

    [Fact]
    public void Options_FileProperty_CanBeSet()
    {
        var options = new Options { File = "custom.cow" };

        options.File.ShouldBe("custom.cow");
    }

    [Fact]
    public void Options_EyesProperty_CanBeSet()
    {
        var options = new Options { Eyes = "@@" };

        options.Eyes.ShouldBe("@@");
    }

    [Fact]
    public void Options_TongueProperty_CanBeSet()
    {
        var options = new Options { Tongue = "U " };

        options.Tongue.ShouldBe("U ");
    }

    [Fact]
    public void Options_WrapProperty_CanBeSet()
    {
        var options = new Options { Wrap = 30 };

        options.Wrap.ShouldBe(30);
    }

    [Fact]
    public void Options_ThinkProperty_CanBeSet()
    {
        var options = new Options { Think = true };

        options.Think.ShouldBeTrue();
    }

    [Fact]
    public void Options_AllProperties_CanBeSetTogether()
    {
        var options = new Options
        {
            Message = "Test",
            List = true,
            Cow = "dragon",
            File = "test.cow",
            Eyes = "xx",
            Tongue = "~~",
            Wrap = 50,
            Think = true
        };

        options.Message.ShouldBe("Test");
        options.List.ShouldBeTrue();
        options.Cow.ShouldBe("dragon");
        options.File.ShouldBe("test.cow");
        options.Eyes.ShouldBe("xx");
        options.Tongue.ShouldBe("~~");
        options.Wrap.ShouldBe(50);
        options.Think.ShouldBeTrue();
    }
}
