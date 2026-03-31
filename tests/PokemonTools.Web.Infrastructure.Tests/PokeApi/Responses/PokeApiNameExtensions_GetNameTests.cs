using PokemonTools.Web.Infrastructure.PokeApi;
using PokemonTools.Web.Infrastructure.PokeApi.Responses;

namespace PokemonTools.Web.Infrastructure.Tests.PokeApi.Responses;

public class PokeApiNameExtensions_GetNameTests
{
    [Fact]
    public void 日本語名が存在する_日本語名が返る()
    {
        // Arrange
        var names = new List<PokeApiLocalizedName>
        {
            new("Stench", new NamedApiResource("en", "https://pokeapi.co/api/v2/language/9/")),
            new("あくしゅう", new NamedApiResource("ja", "https://pokeapi.co/api/v2/language/11/")),
        };

        // Act
        var result = names.GetName("fallback");

        // Assert
        Assert.Equal("あくしゅう", result);
    }

    [Fact]
    public void Jaが存在せずjaHrktが存在する_jaHrktの名前が返る()
    {
        // Arrange
        var names = new List<PokeApiLocalizedName>
        {
            new("Stench", new NamedApiResource("en", "https://pokeapi.co/api/v2/language/9/")),
            new("あくしゅう", new NamedApiResource("ja-hrkt", "https://pokeapi.co/api/v2/language/1/")),
        };

        // Act
        var result = names.GetName("fallback");

        // Assert
        Assert.Equal("あくしゅう", result);
    }

    [Fact]
    public void JaとjaHrktが両方存在する_jaが優先される()
    {
        // Arrange
        var names = new List<PokeApiLocalizedName>
        {
            new("あくしゅう", new NamedApiResource("ja-hrkt", "https://pokeapi.co/api/v2/language/1/")),
            new("悪臭", new NamedApiResource("ja", "https://pokeapi.co/api/v2/language/11/")),
        };

        // Act
        var result = names.GetName("fallback");

        // Assert
        Assert.Equal("悪臭", result);
    }

    [Fact]
    public void JaもjaHrktも存在しない_英語名が返る()
    {
        // Arrange
        var names = new List<PokeApiLocalizedName>
        {
            new("Stench", new NamedApiResource("en", "https://pokeapi.co/api/v2/language/9/")),
        };

        // Act
        var result = names.GetName("stench");

        // Assert
        Assert.Equal("Stench", result);
    }

    [Fact]
    public void 全言語が存在しない_fallbackNameが返る()
    {
        // Arrange
        var names = new List<PokeApiLocalizedName>
        {
            new("Puanteur", new NamedApiResource("fr", "https://pokeapi.co/api/v2/language/5/")),
        };

        // Act
        var result = names.GetName("stench");

        // Assert
        Assert.Equal("stench", result);
    }

    [Fact]
    public void 複数言語が含まれる_日本語名のみ返る()
    {
        // Arrange
        var names = new List<PokeApiLocalizedName>
        {
            new("悪臭", new NamedApiResource("zh-hant", "https://pokeapi.co/api/v2/language/4/")),
            new("Puanteur", new NamedApiResource("fr", "https://pokeapi.co/api/v2/language/5/")),
            new("Stench", new NamedApiResource("en", "https://pokeapi.co/api/v2/language/9/")),
            new("あくしゅう", new NamedApiResource("ja", "https://pokeapi.co/api/v2/language/11/")),
        };

        // Act
        var result = names.GetName("fallback");

        // Assert
        Assert.Equal("あくしゅう", result);
    }
}
