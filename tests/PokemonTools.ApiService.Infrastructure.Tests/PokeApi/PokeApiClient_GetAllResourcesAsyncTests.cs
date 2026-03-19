using Microsoft.Extensions.Time.Testing;
using PokemonTools.ApiService.Infrastructure.PokeApi;
using System.Net;
using System.Text.Json;

namespace PokemonTools.ApiService.Infrastructure.Tests.PokeApi;

public class PokeApiClient_GetAllResourcesAsyncTests
{
    private const string BaseAddress = "https://pokeapi.co/api/v2/";

    [Fact]
    public async Task 単一ページ_全リソースが返る()
    {
        // Arrange
        var responseBody = new
        {
            count = 2,
            next = (string?)null,
            previous = (string?)null,
            results = new[]
            {
                new { name = "bulbasaur", url = $"{BaseAddress}pokemon/1/" },
                new { name = "ivysaur", url = $"{BaseAddress}pokemon/2/" },
            },
        };
        var client = CreateClient((_, _) => CreateJsonResponse(responseBody));

        // Act
        var resources = await CollectAsync(
            client.GetAllResourcesAsync("pokemon", TestContext.Current.CancellationToken));

        // Assert
        Assert.Equal(2, resources.Count);
        Assert.Equal("bulbasaur", resources[0].Name);
        Assert.Equal("ivysaur", resources[1].Name);
    }

    [Fact]
    public async Task 複数ページ_nextを辿って全リソースが返る()
    {
        // Arrange
        var page1 = new
        {
            count = 3,
            next = $"{BaseAddress}pokemon?offset=2&limit=2",
            previous = (string?)null,
            results = new[]
            {
                new { name = "bulbasaur", url = $"{BaseAddress}pokemon/1/" },
                new { name = "ivysaur", url = $"{BaseAddress}pokemon/2/" },
            },
        };
        var page2 = new
        {
            count = 3,
            next = (string?)null,
            previous = $"{BaseAddress}pokemon?offset=0&limit=2",
            results = new[]
            {
                new { name = "venusaur", url = $"{BaseAddress}pokemon/3/" },
            },
        };

        var requestCount = 0;
        var client = CreateClient((request, _) =>
        {
            requestCount++;
            return requestCount == 1
                ? CreateJsonResponse(page1)
                : CreateJsonResponse(page2);
        });

        // Act
        var resources = await CollectAsync(
            client.GetAllResourcesAsync("pokemon", TestContext.Current.CancellationToken));

        // Assert
        Assert.Equal(3, resources.Count);
        Assert.Equal("bulbasaur", resources[0].Name);
        Assert.Equal("ivysaur", resources[1].Name);
        Assert.Equal("venusaur", resources[2].Name);
        Assert.Equal(2, requestCount);
    }

    [Fact]
    public async Task 空のレスポンス_空のシーケンスが返る()
    {
        // Arrange
        var responseBody = new
        {
            count = 0,
            next = (string?)null,
            previous = (string?)null,
            results = Array.Empty<object>(),
        };
        var client = CreateClient((_, _) => CreateJsonResponse(responseBody));

        // Act
        var resources = await CollectAsync(
            client.GetAllResourcesAsync("pokemon", TestContext.Current.CancellationToken));

        // Assert
        Assert.Empty(resources);
    }

    [Fact]
    public async Task HTTPエラー_HttpRequestExceptionがスローされる()
    {
        // Arrange
        var client = CreateClient((_, _) =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError)));

        // Act
        var exception = await Record.ExceptionAsync(
            () => CollectAsync(
                client.GetAllResourcesAsync("pokemon", TestContext.Current.CancellationToken)));

        // Assert
        Assert.IsType<HttpRequestException>(exception);
    }

    private static PokeApiClient CreateClient(
        Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handler)
    {
        var timeProvider = new FakeTimeProvider();
        var limiter = new PokeApiRequestLimiter(timeProvider);
        var mockHandler = new MockHttpMessageHandler((request, ct) =>
        {
            timeProvider.Advance(TimeSpan.FromMilliseconds(200));
            return handler(request, ct);
        });
        var httpClient = new HttpClient(mockHandler) { BaseAddress = new Uri(BaseAddress) };
        return new PokeApiClient(httpClient, limiter);
    }

    private static Task<HttpResponseMessage> CreateJsonResponse(object body)
    {
        var json = JsonSerializer.Serialize(body);
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json"),
        };
        return Task.FromResult(response);
    }

    private static async Task<List<NamedApiResource>> CollectAsync(
        IAsyncEnumerable<NamedApiResource> source)
    {
        var list = new List<NamedApiResource>();
        await foreach (var item in source)
        {
            list.Add(item);
        }
        return list;
    }
}
