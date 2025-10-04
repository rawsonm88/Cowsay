using System;
using System.Collections.Generic;
using Cowsay.CLI.Abstractions;

namespace Cowsay.CLI.Formatters;

public class CowListFormatter : ICowListFormatter
{
    public string Format(IEnumerable<string> cows)
    {
        return string.Join(Environment.NewLine, cows);
    }
}
