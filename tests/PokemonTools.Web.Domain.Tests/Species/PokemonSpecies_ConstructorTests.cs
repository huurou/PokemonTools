using PokemonTools.Web.Domain.Abilities;
using PokemonTools.Web.Domain.Species;
using PokemonTools.Web.Domain.Statistics;
using PokemonTools.Web.Domain.Types;

namespace PokemonTools.Web.Domain.Tests.Species;

public class PokemonSpecies_ConstructorTests
{
    private static readonly TypeId defaultType2Id_ = PokemonType.Ground.Id;

    [Fact]
    public void 正常なパラメータ_PokemonSpeciesが生成される()
    {
        // Act
        var species = CreateWithDefaults();

        // Assert
        Assert.Equal(new SpeciesId(445), species.Id);
        Assert.Equal("ガブリアス", species.Name);
        Assert.Equal(PokemonType.Dragon.Id, species.Type1Id);
        Assert.Equal(PokemonType.Ground.Id, species.Type2Id);
    }

    [Fact]
    public void タイプ2がnull_単タイプのPokemonSpeciesが生成される()
    {
        // Act
        var species = CreateWithDefaults(useType2: false);

        // Assert
        Assert.Null(species.Type2Id);
    }

    [Fact]
    public void タイプ1にステラを指定_例外が発生する()
    {
        // Act
        var exception = Record.Exception(() => CreateWithDefaults(type1Id: PokemonType.Stellar.Id));

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void タイプ1にUnknownを指定_例外が発生する()
    {
        // Act
        var exception = Record.Exception(() => CreateWithDefaults(type1Id: PokemonType.Unknown.Id));

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void タイプ2にステラを指定_例外が発生する()
    {
        // Act
        var exception = Record.Exception(() => CreateWithDefaults(type2Id: PokemonType.Stellar.Id));

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void タイプ2にUnknownを指定_例外が発生する()
    {
        // Act
        var exception = Record.Exception(() => CreateWithDefaults(type2Id: PokemonType.Unknown.Id));

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void タイプ1に無効なIDを指定_例外が発生する()
    {
        // Act
        var exception = Record.Exception(() => CreateWithDefaults(type1Id: new TypeId(999)));

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void タイプ2に無効なIDを指定_例外が発生する()
    {
        // Act
        var exception = Record.Exception(() => CreateWithDefaults(type2Id: new TypeId(999)));

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void 特性2が特性1と同じ_例外が発生する()
    {
        // Act
        var exception = Record.Exception(() =>
            CreateWithDefaults(
                ability1Id: new AbilityId(8),
                ability2Id: new AbilityId(8)
            )
        );

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void 特性2がnull_例外が発生しない()
    {
        // Act
        var species = CreateWithDefaults(ability1Id: new AbilityId(8), ability2Id: null);

        // Assert
        Assert.Null(species.Ability2Id);
    }

    [Fact]
    public void 隠れ特性が特性1と同じ_例外が発生しない()
    {
        // Act
        var species = CreateWithDefaults(
            ability1Id: new AbilityId(8),
            hiddenAbilityId: new AbilityId(8)
        );

        // Assert
        Assert.Equal(new AbilityId(8), species.Ability1Id);
        Assert.Equal(new AbilityId(8), species.HiddenAbilityId);
    }

    [Fact]
    public void タイプ2がタイプ1と同じ_例外が発生する()
    {
        // Act
        var exception = Record.Exception(() =>
            CreateWithDefaults(
                type1Id: PokemonType.Dragon.Id,
                type2Id: PokemonType.Dragon.Id
            )
        );

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }

    private static PokemonSpecies CreateWithDefaults(
        SpeciesId? id = null,
        string name = "ガブリアス",
        TypeId? type1Id = null,
        bool useType2 = true,
        TypeId? type2Id = null,
        AbilityId? ability1Id = null,
        AbilityId? ability2Id = null,
        AbilityId? hiddenAbilityId = null,
        BaseStats? baseStats = null,
        Weight? weight = null
    )
    {
        return new PokemonSpecies(
            id ?? new SpeciesId(445),
            name,
            type1Id ?? PokemonType.Dragon.Id,
            useType2 ? type2Id ?? defaultType2Id_ : null,
            ability1Id ?? new AbilityId(8),
            ability2Id,
            hiddenAbilityId ?? new AbilityId(24),
            baseStats ?? new BaseStats(108, 130, 95, 80, 85, 102),
            weight ?? new Weight(950)
        );
    }
}
