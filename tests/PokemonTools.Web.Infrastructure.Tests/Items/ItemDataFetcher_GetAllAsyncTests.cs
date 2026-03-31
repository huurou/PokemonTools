using Microsoft.Extensions.Time.Testing;
using PokemonTools.Web.Domain.Items;
using PokemonTools.Web.Infrastructure.Items;
using PokemonTools.Web.Infrastructure.PokeApi;
using PokemonTools.Web.Infrastructure.Tests.PokeApi;
using System.Net;
using System.Text.Json;

namespace PokemonTools.Web.Infrastructure.Tests.Items;

public class ItemDataFetcher_GetAllAsyncTests
{
    private const string BASE_ADDRESS = "https://pokeapi.co/api/v2/";

    [Fact]
    public async Task 正常なレスポンス_Itemドメインモデルが返る()
    {
        // Arrange
        var listResponse = new
        {
            count = 1,
            next = (string?)null,
            previous = (string?)null,
            results = new[]
            {
                new { name = "master-ball", url = $"{BASE_ADDRESS}item/1/" },
            },
        };
        var detailResponse = new
        {
            id = 1,
            name = "master-ball",
            names = new[]
            {
                new { name = "Master Ball", language = new { name = "en", url = $"{BASE_ADDRESS}language/9/" } },
                new { name = "マスターボール", language = new { name = "ja", url = $"{BASE_ADDRESS}language/11/" } },
            },
            fling_power = (int?)null,
        };

        var service = CreateService((request, _) =>
            request.RequestUri!.PathAndQuery.Contains("item/1")
                ? CreateJsonResponse(detailResponse)
                : CreateJsonResponse(listResponse));

        // Act
        var items = new List<Domain.Items.Item>();
        await foreach (var item in service.GetAllAsync(TestContext.Current.CancellationToken))
        {
            items.Add(item);
        }

        // Assert
        Assert.Single(items);
        Assert.Equal(1, items[0].Id.Value);
        Assert.Equal("マスターボール", items[0].Name);
        Assert.Null(items[0].FlingPower);
    }

    [Fact]
    public async Task FlingPowerあり_FlingPowerが変換される()
    {
        // Arrange
        var listResponse = new
        {
            count = 1,
            next = (string?)null,
            previous = (string?)null,
            results = new[]
            {
                new { name = "choice-band", url = $"{BASE_ADDRESS}item/233/" },
            },
        };
        var detailResponse = new
        {
            id = 233,
            name = "choice-band",
            names = new[]
            {
                new { name = "こだわりハチマキ", language = new { name = "ja", url = $"{BASE_ADDRESS}language/11/" } },
            },
            fling_power = (int?)10,
        };

        var service = CreateService((request, _) =>
            request.RequestUri!.PathAndQuery.Contains("item/233")
                ? CreateJsonResponse(detailResponse)
                : CreateJsonResponse(listResponse));

        // Act
        var items = new List<Item>();
        await foreach (var item in service.GetAllAsync(TestContext.Current.CancellationToken))
        {
            items.Add(item);
        }

        // Assert
        Assert.Single(items);
        Assert.Equal(10u, items[0].FlingPower);
    }

    private static ItemDataFetcher CreateService(
        Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handler)
    {
        var timeProvider = new FakeTimeProvider();
        var limiter = new PokeApiRequestLimiter(timeProvider);
        var mockHandler = new MockHttpMessageHandler((request, ct) =>
        {
            timeProvider.Advance(TimeSpan.FromMilliseconds(200));
            return handler(request, ct);
        });
        var httpClient = new HttpClient(mockHandler) { BaseAddress = new Uri(BASE_ADDRESS) };
        var pokeApiClient = new PokeApiClient(httpClient, limiter);
        return new ItemDataFetcher(pokeApiClient);
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
