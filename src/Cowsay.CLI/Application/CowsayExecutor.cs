using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Cowsay.CLI.Abstractions;
using Cowsay.CLI.Configuration;
using Cowsay.CLI.Infrastructure;

namespace Cowsay.CLI.Application;

public class CowsayExecutor
{
    private readonly ICowLoader _cowLoader;

    public CowsayExecutor(ICowLoader cowLoader)
    {
        _cowLoader = cowLoader;
    }

    public async Task<CowsayResult> ExecuteAsync(Options options, string message)
    {
        try
        {
            var cow = await _cowLoader.LoadCowAsync(options.File, options.Cow);

            var eyes = options.Eyes ?? CowsayDefaults.DefaultEyes;
            var tongue = options.Tongue ?? CowsayDefaults.DefaultTongue;
            var wrapColumn = options.Wrap ?? CowsayDefaults.DefaultWrapColumn;

            string output;
            if (options.Think)
            {
                output = cow.Think(message, eyes, tongue, wrapColumn);
            }
            else
            {
                output = cow.Speak(message, eyes, tongue, wrapColumn);
            }

            return CowsayResult.Success(output);
        }
        catch (FileNotFoundException)
        {
            if (!string.IsNullOrEmpty(options.File))
            {
                return CowsayResult.Failure(ErrorMessages.FileNotFound(options.File));
            }
            else
            {
                return CowsayResult.Failure(ErrorMessages.CowFormatNotFound(options.Cow));
            }
        }
        catch (HttpRequestException ex)
        {
            return CowsayResult.Failure(ErrorMessages.UrlFetchFailed(options.File!, ex.Message));
        }
        catch (Exception ex)
        {
            return CowsayResult.Failure(ErrorMessages.UnexpectedError(ex.Message));
        }
    }
}
