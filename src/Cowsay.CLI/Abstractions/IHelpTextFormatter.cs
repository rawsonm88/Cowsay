using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace Cowsay.CLI.Abstractions;

public interface IHelpTextFormatter
{
    string FormatHelp<T>(ParserResult<T> result, IEnumerable<Error> errors);
    string FormatVersion();
}
