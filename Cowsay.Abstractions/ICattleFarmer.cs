using System.IO;
using System.Threading.Tasks;

namespace Cowsay.Abstractions
{
    public interface ICattleFarmer
    {
        /// <summary>
        /// Retrieves the format string from the <see cref="ICowFormatProvider" /> and returns a new ICow instance.
        /// </summary>
        /// <param name="cowName">The cow format name.</param>
        /// <returns>A new ICow instance with the format string specified.</returns>
        /// <exception cref="FileNotFoundException">Thrown if the provider can't find the requested cow format.</exception>
        Task<ICow> RearCowAsync(string cowName = "default");

        /// <summary>
        /// Retrieves the format string from the .cow file provided in the stream.
        /// </summary>
        /// <param name="cowStream">The .cow file format stream.</param>
        /// <returns>A new ICow instance with the format string from the stream.</returns>
        Task<ICow> RearCowFromFileStreamAsync(Stream cowStream);
    }
}
