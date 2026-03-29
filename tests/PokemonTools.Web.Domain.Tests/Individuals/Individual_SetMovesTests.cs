using PokemonTools.Web.Domain.Abilities;
using PokemonTools.Web.Domain.Individuals;
using PokemonTools.Web.Domain.Moves;
using PokemonTools.Web.Domain.Species;
using PokemonTools.Web.Domain.Statistics;
using PokemonTools.Web.Domain.Types;

namespace PokemonTools.Web.Domain.Tests.Individuals;

public class Individual_SetMovesTests
{
    [Fact]
    public void 正常な技を指定_技が変更される()
    {
        // Arrange
        var individual = CreateIndividual();

        // Act
        var result = individual.SetMoves(new MoveId(100), new MoveId(200), new MoveId(300), new MoveId(400));

        // Assert
        Assert.Equal(new MoveId(100), result.Move1Id);
        Assert.Equal(new MoveId(200), result.Move2Id);
        Assert.Equal(new MoveId(300), result.Move3Id);
        Assert.Equal(new MoveId(400), result.Move4Id);
    }

    [Fact]
    public void 技以外のプロパティが保持される()
    {
        // Arrange
        var individual = CreateIndividual();

        // Act
        var result = individual.SetMoves(new MoveId(100), null, null, null);

        // Assert
        Assert.Equal(individual.Id, result.Id);
        Assert.Equal(individual.Name, result.Name);
        Assert.Equal(individual.SpeciesId, result.SpeciesId);
        Assert.Equal(individual.StatAlignmentId, result.StatAlignmentId);
        Assert.Equal(individual.AbilityId, result.AbilityId);
        Assert.Equal(individual.StatPoints, result.StatPoints);
        Assert.Equal(individual.HeldItemId, result.HeldItemId);
        Assert.Equal(individual.TeraTypeId, result.TeraTypeId);
        Assert.Equal(individual.Memo, result.Memo);
        Assert.Equal(individual.CategoryId, result.CategoryId);
    }

    [Fact]
    public void 重複する技を指定_例外が発生する()
    {
        // Arrange
        var individual = CreateIndividual();

        // Act
        var exception = Record.Exception(() => individual.SetMoves(new MoveId(100), new MoveId(100), null, null));

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void 技1のみ指定_残りはnullになる()
    {
        // Arrange
        var individual = CreateIndividual();

        // Act
        var result = individual.SetMoves(new MoveId(100), null, null, null);

        // Assert
        Assert.Equal(new MoveId(100), result.Move1Id);
        Assert.Null(result.Move2Id);
        Assert.Null(result.Move3Id);
        Assert.Null(result.Move4Id);
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
