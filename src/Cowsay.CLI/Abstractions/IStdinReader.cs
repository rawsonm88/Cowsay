using System.Threading.Tasks;

namespace Cowsay.CLI.Abstractions;

public interface IStdinReader
{
    Task<string> ReadToEndAsync();
}
