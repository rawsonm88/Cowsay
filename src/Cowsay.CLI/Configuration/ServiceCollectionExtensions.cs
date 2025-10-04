using System.Net.Http;
using Cowsay.CLI.Abstractions;
using Cowsay.CLI.Application;
using Cowsay.CLI.Formatters;
using Cowsay.CLI.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Cowsay.CLI.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCowsayCliServices(this IServiceCollection services)
    {
        services.AddCowsay();

        services.AddSingleton<HttpClient>();
        services.AddSingleton<ICowLoader, CowLoader>();
        services.AddSingleton<CowsayExecutor>();

        services.AddSingleton<IConsoleService, ConsoleService>();
        services.AddSingleton<IStdinReader, StdinReader>();
        services.AddSingleton<IHelpTextFormatter, HelpTextFormatter>();
        services.AddSingleton<ICowListFormatter, CowListFormatter>();

        services.AddTransient<CliRunner>();

        return services;
    }
}
