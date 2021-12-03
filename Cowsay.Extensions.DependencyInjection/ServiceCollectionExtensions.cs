using Cowsay;
using Cowsay.Abstractions;

namespace Microsoft.Extensions.DependencyInjection
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
