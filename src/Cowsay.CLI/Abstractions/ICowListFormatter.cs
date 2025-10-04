using System.Collections.Generic;

namespace Cowsay.CLI.Abstractions;

public interface ICowListFormatter
{
    string Format(IEnumerable<string> cows);
}
