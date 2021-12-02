using System.Threading.Tasks;

namespace Cowsay.Abstractions
{
    public interface ICowFormatProvider
    {
        Task<string> GetCowFormatAsync(string cowName);
    }
}
