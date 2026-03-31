using PokemonTools.Web.Infrastructure.PokeApi.Responses;
using System.Text.Json;

namespace PokemonTools.Web.Infrastructure.Tests.PokeApi.Responses;

public class PokeApiPokemonSpeciesResponse_DeserializeTests
{
    private static readonly JsonSerializerOptions jsonOptions_ = new(JsonSerializerDefaults.Web)
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
    };

    [Fact]
    public void 正常なJSON_各プロパティが正しくデシリアライズされる()
    {
        // Arrange
        var json = """
            {
                "id": 1,
                "name": "bulbasaur",
                "names": [
                    {
                        "name": "Bulbasaur",
                        "language": { "name": "en", "url": "https://pokeapi.co/api/v2/language/9/" }
                    },
                    {
                        "name": "フシギダネ",
                        "language": { "name": "ja", "url": "https://pokeapi.co/api/v2/language/11/" }
                    }
                ]
            }
            """;

        // Act
        var result = JsonSerializer.Deserialize<PokeApiPokemonSpeciesResponse>(json, jsonOptions_);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("bulbasaur", result.Name);
        Assert.Equal("フシギダネ", result.Names.GetName(result.Name));
    }
}
