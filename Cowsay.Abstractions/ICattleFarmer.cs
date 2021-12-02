using System.IO;
using System.Threading.Tasks;

namespace Cowsay.Abstractions
{
    public interface ICattleFarmer
    {
        Task<ICow> RearCowAsync(string cowName);
        Task<ICow> RearCowFromFileStreamAsync(Stream cowStream);
    }
}
