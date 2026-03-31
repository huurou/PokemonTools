using PokemonTools.Web.Infrastructure.PokeApi.Responses;
using System.Text.Json;

namespace PokemonTools.Web.Infrastructure.Tests.PokeApi.Responses;

public class PokeApiItemResponse_DeserializeTests
{
    private static readonly JsonSerializerOptions jsonOptions_ = new(JsonSerializerDefaults.Web)
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
    };

    [Fact]
    public void Fling_powerがnull_FlingPowerがnullでデシリアライズされる()
    {
        // Arrange
        var json = """
            {
                "id": 1,
                "name": "master-ball",
                "names": [
                    {
                        "name": "マスターボール",
                        "language": { "name": "ja", "url": "https://pokeapi.co/api/v2/language/11/" }
                    }
                ],
                "fling_power": null
            }
            """;

        // Act
        var result = JsonSerializer.Deserialize<PokeApiItemResponse>(json, jsonOptions_);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("master-ball", result.Name);
        Assert.Null(result.FlingPower);
        Assert.Equal("マスターボール", result.Names.GetName(result.Name));
    }

    [Fact]
    public void Fling_powerが値あり_FlingPowerが正しくデシリアライズされる()
    {
        // Arrange
        var json = """
            {
                "id": 233,
                "name": "choice-band",
                "names": [
                    {
                        "name": "こだわりハチマキ",
                        "language": { "name": "ja", "url": "https://pokeapi.co/api/v2/language/11/" }
                    }
                ],
                "fling_power": 10
            }
            """;

        // Act
        var result = JsonSerializer.Deserialize<PokeApiItemResponse>(json, jsonOptions_);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(233, result.Id);
        Assert.Equal(10, result.FlingPower);
    }
}
