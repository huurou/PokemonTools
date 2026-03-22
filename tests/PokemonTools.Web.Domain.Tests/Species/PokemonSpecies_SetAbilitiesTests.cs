using PokemonTools.Web.Domain.Abilities;
using PokemonTools.Web.Domain.Species;
using PokemonTools.Web.Domain.Statistics;
using PokemonTools.Web.Domain.Types;

namespace PokemonTools.Web.Domain.Tests.Species;

public class PokemonSpecies_SetAbilitiesTests
{
    [Fact]
    public void 正常な特性を指定_特性が変更される()
    {
        // Arrange
        var species = CreateSpecies();

        // Act
        var result = species.SetAbilities(new AbilityId(10), new AbilityId(20));

        // Assert
        Assert.Equal(new AbilityId(10), result.Ability1Id);
        Assert.Equal(new AbilityId(20), result.Ability2Id);
    }

    [Fact]
    public void 特性以外のプロパティが保持される()
    {
        // Arrange
        var species = CreateSpecies();

        // Act
        var result = species.SetAbilities(new AbilityId(10), new AbilityId(20));

        // Assert
        Assert.Equal(species.Id, result.Id);
        Assert.Equal(species.Name, result.Name);
        Assert.Equal(species.Type1Id, result.Type1Id);
        Assert.Equal(species.Type2Id, result.Type2Id);
        Assert.Equal(species.HiddenAbilityId, result.HiddenAbilityId);
        Assert.Equal(species.BaseStats, result.BaseStats);
        Assert.Equal(species.Weight, result.Weight);
    }

    [Fact]
    public void 特性2が特性1と同じ_例外が発生する()
    {
        // Arrange
        var species = CreateSpecies();

        // Act
        var exception = Record.Exception(() => species.SetAbilities(new AbilityId(10), new AbilityId(10)));

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void 特性2にnullを指定_特性1のみになる()
    {
        // Arrange
        var species = CreateSpecies();

        // Act
        var result = species.SetAbilities(new AbilityId(10), null);

        // Assert
        Assert.Equal(new AbilityId(10), result.Ability1Id);
        Assert.Null(result.Ability2Id);
    }

    private static PokemonSpecies CreateSpecies()
    {
        return new PokemonSpecies(
            new SpeciesId(445),
            "ガブリアス",
            PokemonType.Dragon.Id,
            PokemonType.Ground.Id,
            new AbilityId(8),
            null,
            new AbilityId(24),
            new BaseStats(108, 130, 95, 80, 85, 102),
            new Weight(950)
        );
    }
}
