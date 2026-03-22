using Microsoft.Extensions.Time.Testing;
using PokemonTools.Web.Infrastructure.PokeApi;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PokemonTools.Web.Infrastructure.Tests.PokeApi;

public class PokeApiClient_GetResourceAsyncTests
{
    private const string BASE_ADDRESS = "https://pokeapi.co/api/v2/";

    [Fact]
    public async Task エンドポイントと名前で取得_正しくデシリアライズされる()
    {
        // Arrange
        var responseBody = new { id = 25, name = "pikachu" };
        var requestedUrl = "";
        var client = CreateClient((request, _) =>
        {
            requestedUrl = request.RequestUri!.ToString();
            return CreateJsonResponse(responseBody);
        });

        // Act
        var result = await client.GetResourceAsync<TestResource>("pokemon", "pikachu", TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(25, result.Id);
        Assert.Equal("pikachu", result.Name);
        Assert.Equal($"{BASE_ADDRESS}pokemon/pikachu", requestedUrl);
    }

    [Fact]
    public async Task NamedApiResourceで取得_URLで取得できる()
    {
        // Arrange
        var resource = new NamedApiResource("pikachu", $"{BASE_ADDRESS}pokemon/25/");
        var responseBody = new { id = 25, name = "pikachu" };
        var requestedUrl = "";
        var client = CreateClient(
            (request, _) =>
            {
                requestedUrl = request.RequestUri!.ToString();
                return CreateJsonResponse(responseBody);
            }
        );

        // Act
        var result = await client.GetResourceAsync<TestResource>(resource, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(25, result.Id);
        Assert.Equal("pikachu", result.Name);
        Assert.Equal(resource.Url, requestedUrl);
    }

    [Fact]
    public async Task NamedApiResourceのURLがBaseAddress外_ArgumentExceptionがスローされる()
    {
        // Arrange
        var resource = new NamedApiResource("pikachu", "https://evil.example.com/pokemon/25/");
        var client = CreateClient((_, _) =>
            CreateJsonResponse(new { id = 25, name = "pikachu" }));

        // Act
        var exception = await Record.ExceptionAsync(
            () => client.GetResourceAsync<TestResource>(resource, TestContext.Current.CancellationToken)
        );

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public async Task HTTPエラー_HttpRequestExceptionがスローされる()
    {
        // Arrange
        var client = CreateClient((_, _) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound)));

        // Act
        var exception = await Record.ExceptionAsync(
            () => client.GetResourceAsync<TestResource>("pokemon", "nonexistent", TestContext.Current.CancellationToken)
        );

        // Assert
        Assert.IsType<HttpRequestException>(exception);
    }

    private static PokeApiClient CreateClient(
        Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handler)
    {
        var timeProvider = new FakeTimeProvider();
        var limiter = new PokeApiRequestLimiter(timeProvider);
        var mockHandler = new MockHttpMessageHandler(
            (request, ct) =>
            {
                timeProvider.Advance(TimeSpan.FromMilliseconds(200));
                return handler(request, ct);
            }
        );
        var httpClient = new HttpClient(mockHandler) { BaseAddress = new Uri(BASE_ADDRESS) };
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
}

internal record TestResource(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name
);
