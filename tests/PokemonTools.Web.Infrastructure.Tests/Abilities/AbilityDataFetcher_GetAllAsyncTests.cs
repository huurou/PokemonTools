using Microsoft.Extensions.Time.Testing;
using PokemonTools.Web.Domain.Abilities;
using PokemonTools.Web.Infrastructure.Abilities;
using PokemonTools.Web.Infrastructure.PokeApi;
using PokemonTools.Web.Infrastructure.Tests.PokeApi;
using System.Net;
using System.Text.Json;

namespace PokemonTools.Web.Infrastructure.Tests.Abilities;

public class AbilityDataFetcher_GetAllAsyncTests
{
    private const string BASE_ADDRESS = "https://pokeapi.co/api/v2/";

    [Fact]
    public async Task 正常なレスポンス_Abilityドメインモデルが返る()
    {
        // Arrange
        var listResponse = new
        {
            count = 1,
            next = (string?)null,
            previous = (string?)null,
            results = new[]
            {
                new { name = "stench", url = $"{BASE_ADDRESS}ability/1/" },
            },
        };
        var detailResponse = new
        {
            id = 1,
            name = "stench",
            names = new[]
            {
                new { name = "Stench", language = new { name = "en", url = $"{BASE_ADDRESS}language/9/" } },
                new { name = "あくしゅう", language = new { name = "ja", url = $"{BASE_ADDRESS}language/11/" } },
            },
        };

        var service = CreateService((request, _) =>
            request.RequestUri!.PathAndQuery.Contains("ability/1")
                ? CreateJsonResponse(detailResponse)
                : CreateJsonResponse(listResponse));

        // Act
        var abilities = new List<Ability>();
        await foreach (var ability in service.GetAllAsync(TestContext.Current.CancellationToken))
        {
            abilities.Add(ability);
        }

        // Assert
        Assert.Single(abilities);
        Assert.Equal(1, abilities[0].Id.Value);
        Assert.Equal("あくしゅう", abilities[0].Name);
    }

    private static AbilityDataFetcher CreateService(
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
        return new AbilityDataFetcher(pokeApiClient);
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
