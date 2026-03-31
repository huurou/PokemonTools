using PokemonTools.Web.Infrastructure.PokeApi.Responses;
using System.Text.Json;

namespace PokemonTools.Web.Infrastructure.Tests.PokeApi.Responses;

public class PokeApiMoveResponse_DeserializeTests
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
                "name": "pound",
                "names": [
                    {
                        "name": "はたく",
                        "language": { "name": "ja", "url": "https://pokeapi.co/api/v2/language/11/" }
                    }
                ],
                "type": { "name": "normal", "url": "https://pokeapi.co/api/v2/type/1/" },
                "damage_class": { "name": "physical", "url": "https://pokeapi.co/api/v2/move-damage-class/2/" },
                "power": 40
            }
            """;

        // Act
        var result = JsonSerializer.Deserialize<PokeApiMoveResponse>(json, jsonOptions_);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("はたく", result.Names.GetName(result.Name));
        Assert.Equal("normal", result.Type.Name);
        Assert.Equal("physical", result.DamageClass.Name);
        Assert.Equal(40, result.Power);
    }

    [Fact]
    public void Powerがnull_Powerがnullでデシリアライズされる()
    {
        // Arrange
        var json = """
            {
                "id": 14,
                "name": "swords-dance",
                "names": [
                    {
                        "name": "つるぎのまい",
                        "language": { "name": "ja", "url": "https://pokeapi.co/api/v2/language/11/" }
                    }
                ],
                "type": { "name": "normal", "url": "https://pokeapi.co/api/v2/type/1/" },
                "damage_class": { "name": "status", "url": "https://pokeapi.co/api/v2/move-damage-class/1/" },
                "power": null
            }
            """;

        // Act
        var result = JsonSerializer.Deserialize<PokeApiMoveResponse>(json, jsonOptions_);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.Power);
        Assert.Equal("status", result.DamageClass.Name);
    }
}
