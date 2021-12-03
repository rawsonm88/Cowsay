using Cowsay.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Cowsay.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCowsay(this IServiceCollection services)
        {
            return services
                .AddSingleton<ICowFormatProvider, EmbeddedCowFormatProvider>()
                .AddSingleton<IBubbleBlower, DefaultBubbleBlower>()
                .AddSingleton<ICattleFarmer, DefaultCattleFarmer>();
        }
    }
}
