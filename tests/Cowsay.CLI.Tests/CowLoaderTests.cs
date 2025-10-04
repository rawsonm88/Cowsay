using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cowsay.Abstractions;
using Cowsay.CLI.Infrastructure;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Cowsay.CLI.Tests;

public class CowLoaderTests
{
    [Fact]
    public async Task LoadCowAsync_WithNullFile_LoadsFromEmbedded()
    {
        var cattleFarmer = Substitute.For<ICattleFarmer>();
        var expectedCow = Substitute.For<ICow>();
        cattleFarmer.RearCowAsync("default").Returns(expectedCow);

        var httpClient = new HttpClient();
        var cowLoader = new CowLoader(cattleFarmer, httpClient);

        var cow = await cowLoader.LoadCowAsync(null, "default");

        cow.ShouldBe(expectedCow);
        await cattleFarmer.Received(1).RearCowAsync("default");
    }

    [Fact]
    public async Task LoadCowAsync_WithEmptyFile_LoadsFromEmbedded()
    {
        var cattleFarmer = Substitute.For<ICattleFarmer>();
        var expectedCow = Substitute.For<ICow>();
        cattleFarmer.RearCowAsync("tux").Returns(expectedCow);

        var httpClient = new HttpClient();
        var cowLoader = new CowLoader(cattleFarmer, httpClient);

        var cow = await cowLoader.LoadCowAsync(string.Empty, "tux");

        cow.ShouldBe(expectedCow);
        await cattleFarmer.Received(1).RearCowAsync("tux");
    }

    [Fact]
    public async Task LoadCowAsync_WithFilePath_LoadsFromFile()
    {
        var cattleFarmer = Substitute.For<ICattleFarmer>();
        var expectedCow = Substitute.For<ICow>();
        cattleFarmer.RearCowFromFileStreamAsync(Arg.Any<Stream>()).Returns(expectedCow);

        var httpClient = new HttpClient();
        var cowLoader = new CowLoader(cattleFarmer, httpClient);

        var testFile = Path.GetTempFileName();
        await File.WriteAllTextAsync(testFile, "$the_cow = <<EOC;\r\nTest$eye$eye\r\nEOC", TestContext.Current.CancellationToken);

        try
        {
            var cow = await cowLoader.LoadCowAsync(testFile, "default");

            cow.ShouldBe(expectedCow);
            await cattleFarmer.Received(1).RearCowFromFileStreamAsync(Arg.Any<Stream>());
        }
        finally
        {
            File.Delete(testFile);
        }
    }

    [Fact]
    public async Task LoadCowAsync_WithHttpUrl_LoadsFromUrl()
    {
        var cattleFarmer = Substitute.For<ICattleFarmer>();
        var expectedCow = Substitute.For<ICow>();
        cattleFarmer.RearCowFromFileStreamAsync(Arg.Any<Stream>()).Returns(expectedCow);

        var mockHandler = new MockHttpMessageHandler(
            "$the_cow = <<EOC;\r\nURL$eye$eye\r\nEOC",
            HttpStatusCode.OK
        );
        var httpClient = new HttpClient(mockHandler);
        var cowLoader = new CowLoader(cattleFarmer, httpClient);

        var cow = await cowLoader.LoadCowAsync("http://example.com/test.cow", "default");

        cow.ShouldBe(expectedCow);
        await cattleFarmer.Received(1).RearCowFromFileStreamAsync(Arg.Any<Stream>());
    }

    [Fact]
    public async Task LoadCowAsync_WithHttpsUrl_LoadsFromUrl()
    {
        var cattleFarmer = Substitute.For<ICattleFarmer>();
        var expectedCow = Substitute.For<ICow>();
        cattleFarmer.RearCowFromFileStreamAsync(Arg.Any<Stream>()).Returns(expectedCow);

        var mockHandler = new MockHttpMessageHandler(
            "$the_cow = <<EOC;\r\nSecure$eye$eye\r\nEOC",
            HttpStatusCode.OK
        );
        var httpClient = new HttpClient(mockHandler);
        var cowLoader = new CowLoader(cattleFarmer, httpClient);

        var cow = await cowLoader.LoadCowAsync("https://example.com/secure.cow", "default");

        cow.ShouldBe(expectedCow);
        await cattleFarmer.Received(1).RearCowFromFileStreamAsync(Arg.Any<Stream>());
    }

    private class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _content;
        private readonly HttpStatusCode _statusCode;

        public MockHttpMessageHandler(string content, HttpStatusCode statusCode)
        {
            _content = content;
            _statusCode = statusCode;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(_content, Encoding.UTF8)
            };
            return Task.FromResult(response);
        }
    }
}
