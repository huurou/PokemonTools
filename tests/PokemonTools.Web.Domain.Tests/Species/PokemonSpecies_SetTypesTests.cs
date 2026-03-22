using PokemonTools.Web.Domain.Abilities;
using PokemonTools.Web.Domain.Species;
using PokemonTools.Web.Domain.Statistics;
using PokemonTools.Web.Domain.Types;

namespace PokemonTools.Web.Domain.Tests.Species;

public class PokemonSpecies_SetTypesTests
{
    [Fact]
    public void 正常なタイプを指定_タイプが変更される()
    {
        // Arrange
        var species = CreateSpecies();

        // Act
        var result = species.SetTypes(PokemonType.Fire.Id, PokemonType.Flying.Id);

        // Assert
        Assert.Equal(PokemonType.Fire.Id, result.Type1Id);
        Assert.Equal(PokemonType.Flying.Id, result.Type2Id);
    }

    [Fact]
    public void タイプ以外のプロパティが保持される()
    {
        // Arrange
        var species = CreateSpecies();

        // Act
        var result = species.SetTypes(PokemonType.Fire.Id, PokemonType.Flying.Id);

        // Assert
        Assert.Equal(species.Id, result.Id);
        Assert.Equal(species.Name, result.Name);
        Assert.Equal(species.Ability1Id, result.Ability1Id);
        Assert.Equal(species.Ability2Id, result.Ability2Id);
        Assert.Equal(species.HiddenAbilityId, result.HiddenAbilityId);
        Assert.Equal(species.BaseStats, result.BaseStats);
        Assert.Equal(species.Weight, result.Weight);
    }

    [Fact]
    public void タイプ2がタイプ1と同じ_例外が発生する()
    {
        // Arrange
        var species = CreateSpecies();

        // Act
        var exception = Record.Exception(() => species.SetTypes(PokemonType.Fire.Id, PokemonType.Fire.Id));

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void タイプ2にnullを指定_単タイプになる()
    {
        // Arrange
        var species = CreateSpecies();

        // Act
        var result = species.SetTypes(PokemonType.Fire.Id, null);

        // Assert
        Assert.Equal(PokemonType.Fire.Id, result.Type1Id);
        Assert.Null(result.Type2Id);
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
