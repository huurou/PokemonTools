using PokemonTools.Web.Domain.Abilities;
using PokemonTools.Web.Domain.Individuals;
using PokemonTools.Web.Domain.Items;
using PokemonTools.Web.Domain.Moves;
using PokemonTools.Web.Domain.Species;
using PokemonTools.Web.Domain.Statistics;
using PokemonTools.Web.Domain.Types;

namespace PokemonTools.Web.Domain.Tests.Individuals;

public class Individual_ConstructorTests
{
    [Fact]
    public void 正常なパラメータ_Individualが生成される()
    {
        // Act
        var individual = CreateWithDefaults();

        // Assert
        Assert.Equal(new IndividualId("ind_test"), individual.Id);
        Assert.Equal(new SpeciesId(445), individual.SpeciesId);
        Assert.Equal(new MoveId(89), individual.Move1Id);
    }

    [Fact]
    public void 技1のみ指定_Individualが生成される()
    {
        // Act
        var individual = CreateWithDefaults();

        // Assert
        Assert.Equal(new MoveId(89), individual.Move1Id);
        Assert.Null(individual.Move2Id);
        Assert.Null(individual.Move3Id);
        Assert.Null(individual.Move4Id);
    }

    [Fact]
    public void 技を4件指定_Individualが生成される()
    {
        // Act
        var individual = CreateWithDefaults(
            move1Id: new MoveId(89),
            move2Id: new MoveId(200),
            move3Id: new MoveId(337),
            move4Id: new MoveId(406)
        );

        // Assert
        Assert.Equal(new MoveId(89), individual.Move1Id);
        Assert.Equal(new MoveId(200), individual.Move2Id);
        Assert.Equal(new MoveId(337), individual.Move3Id);
        Assert.Equal(new MoveId(406), individual.Move4Id);
    }

    [Fact]
    public void テラスタイプにステラを指定_Individualが生成される()
    {
        // Act
        var individual = CreateWithDefaults(teraTypeId: PokemonType.Stellar.Id);

        // Assert
        Assert.Equal(PokemonType.Stellar.Id, individual.TeraTypeId);
    }

    [Fact]
    public void テラスタイプにUnknownを指定_例外が発生する()
    {
        // Act
        var exception = Record.Exception(() => CreateWithDefaults(teraTypeId: PokemonType.Unknown.Id));

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void テラスタイプに無効なIDを指定_例外が発生する()
    {
        // Act
        var exception = Record.Exception(() => CreateWithDefaults(teraTypeId: new TypeId(999)));

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void 無効な性格IDを指定_例外が発生する()
    {
        // Act
        var exception = Record.Exception(() => CreateWithDefaults(natureId: new NatureId(999)));

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void 無効な個体カテゴリIDを指定_例外が発生する()
    {
        // Act
        var exception = Record.Exception(() => CreateWithDefaults(categoryId: new IndividualCategoryId(999)));

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }

    private static Individual CreateWithDefaults(
        IndividualId? id = null,
        string? name = null,
        SpeciesId? speciesId = null,
        NatureId? natureId = null,
        AbilityId? abilityId = null,
        IndividualValues? individualValues = null,
        EffortValues? effortValues = null,
        MoveId? move1Id = null,
        MoveId? move2Id = null,
        MoveId? move3Id = null,
        MoveId? move4Id = null,
        ItemId? heldItemId = null,
        TypeId? teraTypeId = null,
        string? memo = null,
        IndividualCategoryId? categoryId = null
    )
    {
        return new Individual(
            id ?? new IndividualId("ind_test"),
            name,
            speciesId ?? new SpeciesId(445),
            natureId ?? new NatureId(11),
            abilityId ?? new AbilityId(8),
            individualValues ?? new IndividualValues(31, 31, 31, 31, 31, 31),
            effortValues ?? new EffortValues(0, 252, 0, 0, 4, 252),
            move1Id ?? new MoveId(89),
            move2Id,
            move3Id,
            move4Id,
            heldItemId,
            teraTypeId ?? PokemonType.Dragon.Id,
            memo,
            categoryId ?? IndividualCategory.OwnedIndividual.Id
        );
    }
}
