using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cowsay.Abstractions
{
    public interface ICowFormatProvider
    {
        /// <summary>
        /// Gets the format string for the cow which includes placeholders for thing like $eyes and $thoughts.
        /// </summary>
        /// <param name="cowName">The name of type of cow to retrieve.</param>
        /// <returns>The format string for the cow.</returns>
        Task<string> GetCowFormatAsync(string cowName);

        /// <summary>
        /// Gets a read-only list of available cow format names.
        /// </summary>
        /// <returns>A read-only list of available cow format names.</returns>
        Task<IReadOnlyList<string>> GetAvailableCowsAsync();
    }
}
