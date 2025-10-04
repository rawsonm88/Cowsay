using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Cowsay.Abstractions;
using Cowsay.CLI.Abstractions;

namespace Cowsay.CLI.Infrastructure;

public class CowLoader : ICowLoader
{
    private readonly ICattleFarmer _cattleFarmer;
    private readonly HttpClient _httpClient;

    public CowLoader(ICattleFarmer cattleFarmer, HttpClient httpClient)
    {
        _cattleFarmer = cattleFarmer;
        _httpClient = httpClient;
    }

    public async Task<ICow> LoadCowAsync(string? filePath, string cowName)
    {
        if (!string.IsNullOrEmpty(filePath))
        {
            if (Uri.TryCreate(filePath, UriKind.Absolute, out var uri) &&
                (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            {
                return await LoadCowFromUrlAsync(uri);
            }
            else
            {
                return await LoadCowFromFileAsync(filePath);
            }
        }
        else
        {
            return await _cattleFarmer.RearCowAsync(cowName);
        }
    }

    private async Task<ICow> LoadCowFromUrlAsync(Uri uri)
    {
        var content = await _httpClient.GetStringAsync(uri);
        await using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
        return await _cattleFarmer.RearCowFromFileStreamAsync(stream);
    }

    private async Task<ICow> LoadCowFromFileAsync(string filePath)
    {
        await using var fileStream = File.OpenRead(filePath);
        return await _cattleFarmer.RearCowFromFileStreamAsync(fileStream);
    }
}
