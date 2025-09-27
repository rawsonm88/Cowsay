using Microsoft.Extensions.DependencyInjection;
using Cowsay.Abstractions;
using Shouldly;
using Xunit;

namespace Cowsay.Extensions.DependencyInjection.Tests
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddCowsay_registers_all_required_services()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddCowsay();
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            serviceProvider.GetService<ICowFormatProvider>().ShouldNotBeNull();
            serviceProvider.GetService<IBubbleBlower>().ShouldNotBeNull();
            serviceProvider.GetService<ICattleFarmer>().ShouldNotBeNull();
        }

        [Fact]
        public void AddCowsay_registers_services_as_singletons()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddCowsay();
            var serviceProvider = services.BuildServiceProvider();

            // Assert - get services twice and verify same instance
            var provider1 = serviceProvider.GetService<ICowFormatProvider>();
            var provider2 = serviceProvider.GetService<ICowFormatProvider>();
            provider1.ShouldBeSameAs(provider2);

            var blower1 = serviceProvider.GetService<IBubbleBlower>();
            var blower2 = serviceProvider.GetService<IBubbleBlower>();
            blower1.ShouldBeSameAs(blower2);

            var farmer1 = serviceProvider.GetService<ICattleFarmer>();
            var farmer2 = serviceProvider.GetService<ICattleFarmer>();
            farmer1.ShouldBeSameAs(farmer2);
        }

        [Fact]
        public void AddCowsay_registers_correct_implementations()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddCowsay();
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            serviceProvider.GetService<ICowFormatProvider>().ShouldBeOfType<EmbeddedCowFormatProvider>();
            serviceProvider.GetService<IBubbleBlower>().ShouldBeOfType<DefaultBubbleBlower>();
            serviceProvider.GetService<ICattleFarmer>().ShouldBeOfType<DefaultCattleFarmer>();
        }

        [Fact]
        public void AddCowsay_returns_service_collection_for_chaining()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result = services.AddCowsay();

            // Assert
            result.ShouldBeSameAs(services);
        }

        [Fact]
        public async Task Can_create_cow_using_registered_services()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddCowsay();
            var serviceProvider = services.BuildServiceProvider();

            // Act
            var cattleFarmer = serviceProvider.GetRequiredService<ICattleFarmer>();
            var cow = await cattleFarmer.RearCowAsync("default");

            // Assert
            cow.ShouldNotBeNull();
            cow.Format.ShouldNotBeNullOrEmpty();
        }
    }
}