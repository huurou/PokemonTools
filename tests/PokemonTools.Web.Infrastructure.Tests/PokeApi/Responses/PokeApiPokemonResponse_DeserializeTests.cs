using PokemonTools.Web.Infrastructure.PokeApi.Responses;
using System.Text.Json;

namespace PokemonTools.Web.Infrastructure.Tests.PokeApi.Responses;

public class PokeApiPokemonResponse_DeserializeTests
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
                "types": [
                    { "slot": 1, "type": { "name": "grass", "url": "https://pokeapi.co/api/v2/type/12/" } },
                    { "slot": 2, "type": { "name": "poison", "url": "https://pokeapi.co/api/v2/type/4/" } }
                ],
                "abilities": [
                    { "slot": 1, "is_hidden": false, "ability": { "name": "overgrow", "url": "https://pokeapi.co/api/v2/ability/65/" } },
                    { "slot": 3, "is_hidden": true, "ability": { "name": "chlorophyll", "url": "https://pokeapi.co/api/v2/ability/34/" } }
                ],
                "stats": [
                    { "base_stat": 45, "stat": { "name": "hp", "url": "https://pokeapi.co/api/v2/stat/1/" } },
                    { "base_stat": 49, "stat": { "name": "attack", "url": "https://pokeapi.co/api/v2/stat/2/" } },
                    { "base_stat": 49, "stat": { "name": "defense", "url": "https://pokeapi.co/api/v2/stat/3/" } },
                    { "base_stat": 65, "stat": { "name": "special-attack", "url": "https://pokeapi.co/api/v2/stat/4/" } },
                    { "base_stat": 65, "stat": { "name": "special-defense", "url": "https://pokeapi.co/api/v2/stat/5/" } },
                    { "base_stat": 45, "stat": { "name": "speed", "url": "https://pokeapi.co/api/v2/stat/6/" } }
                ],
                "forms": [
                    { "name": "bulbasaur", "url": "https://pokeapi.co/api/v2/pokemon-form/1/" }
                ],
                "weight": 69,
                "species": { "name": "bulbasaur", "url": "https://pokeapi.co/api/v2/pokemon-species/1/" }
            }
            """;

        // Act
        var result = JsonSerializer.Deserialize<PokeApiPokemonResponse>(json, jsonOptions_);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("bulbasaur", result.Name);
        Assert.Equal(2, result.Types.Count);
        Assert.Equal(1, result.Types[0].Slot);
        Assert.Equal("grass", result.Types[0].Type.Name);
        Assert.Equal(2, result.Abilities.Count);
        Assert.False(result.Abilities[0].IsHidden);
        Assert.True(result.Abilities[1].IsHidden);
        Assert.Equal(6, result.Stats.Count);
        Assert.Equal(45, result.Stats[0].BaseStat);
        Assert.Equal("hp", result.Stats[0].Stat.Name);
        Assert.Single(result.Forms);
        Assert.Equal("bulbasaur", result.Forms[0].Name);
        Assert.Equal(69, result.Weight);
        Assert.Equal("bulbasaur", result.Species.Name);
    }
}
