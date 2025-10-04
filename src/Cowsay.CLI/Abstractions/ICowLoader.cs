using System.Threading.Tasks;
using Cowsay.Abstractions;

namespace Cowsay.CLI.Abstractions;

public interface ICowLoader
{
    Task<ICow> LoadCowAsync(string? filePath, string cowName);
}
