using PokemonTools.Web.Domain.Abilities;
using PokemonTools.Web.Domain.Individuals;
using PokemonTools.Web.Domain.Items;
using PokemonTools.Web.Domain.Moves;
using PokemonTools.Web.Domain.Species;
using PokemonTools.Web.Domain.Statistics;
using PokemonTools.Web.Domain.Types;

namespace PokemonTools.Web.Domain.Tests.Individuals;

public class Individual_UpdateTests
{
    [Fact]
    public void 正常な値を指定_フィールドが更新される()
    {
        // Arrange
        var individual = CreateIndividual();

        // Act
        var result = individual.Update(
            "ガブリアス改",
            new SpeciesId(448),
            new StatAlignmentId(13),
            new AbilityId(10),
            new StatPoints(0, 0, 0, 32, 0, 32),
            new MoveId(100),
            new MoveId(200),
            null,
            null,
            new ItemId(50),
            PokemonType.Fire.Id,
            "メモ更新");

        // Assert
        Assert.Equal("ガブリアス改", result.Name);
        Assert.Equal(new SpeciesId(448), result.SpeciesId);
        Assert.Equal(new StatAlignmentId(13), result.StatAlignmentId);
        Assert.Equal(new AbilityId(10), result.AbilityId);
        Assert.Equal(new StatPoints(0, 0, 0, 32, 0, 32), result.StatPoints);
        Assert.Equal(new MoveId(100), result.Move1Id);
        Assert.Equal(new MoveId(200), result.Move2Id);
        Assert.Null(result.Move3Id);
        Assert.Null(result.Move4Id);
        Assert.Equal(new ItemId(50), result.HeldItemId);
        Assert.Equal(PokemonType.Fire.Id, result.TeraTypeId);
        Assert.Equal("メモ更新", result.Memo);
    }

    [Fact]
    public void IdとCategoryIdが保持される()
    {
        // Arrange
        var individual = CreateIndividual();

        // Act
        var result = individual.Update(
            "更新後",
            new SpeciesId(448),
            new StatAlignmentId(11),
            new AbilityId(8),
            new StatPoints(0, 32, 0, 0, 2, 32),
            new MoveId(100),
            null,
            null,
            null,
            null,
            PokemonType.Dragon.Id,
            null);

        // Assert
        Assert.Equal(individual.Id, result.Id);
        Assert.Equal(individual.CategoryId, result.CategoryId);
    }

    [Fact]
    public void 重複する技を指定_例外が発生する()
    {
        // Arrange
        var individual = CreateIndividual();

        // Act
        var exception = Record.Exception(() => individual.Update(
            null,
            new SpeciesId(445),
            new StatAlignmentId(11),
            new AbilityId(8),
            new StatPoints(0, 32, 0, 0, 2, 32),
            new MoveId(100),
            new MoveId(100),
            null,
            null,
            null,
            PokemonType.Dragon.Id,
            null));

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }

    private static Individual CreateIndividual()
    {
        return new Individual(
            new IndividualId("ind_test"),
            null,
            new SpeciesId(445),
            new StatAlignmentId(11),
            new AbilityId(8),
            new StatPoints(0, 32, 0, 0, 2, 32),
            new MoveId(89),
            null,
            null,
            null,
            null,
            PokemonType.Dragon.Id,
            null,
            IndividualCategory.OwnedIndividual.Id
        );
    }
}
