using PokemonTools.Web.Domain.Individuals;
using PokemonTools.Web.Domain.Parties;

namespace PokemonTools.Web.Domain.Tests.Parties;

public class Party_SetIndividualsTests
{
    [Fact]
    public void 正常な個体を指定_個体が変更される()
    {
        // Arrange
        var party = CreateParty();

        // Act
        var result = party.SetIndividuals(
            new IndividualId("ind_1"), new IndividualId("ind_2"), new IndividualId("ind_3"),
            new IndividualId("ind_4"), new IndividualId("ind_5"), new IndividualId("ind_6"));

        // Assert
        Assert.Equal(new IndividualId("ind_1"), result.Individual1Id);
        Assert.Equal(new IndividualId("ind_2"), result.Individual2Id);
        Assert.Equal(new IndividualId("ind_3"), result.Individual3Id);
        Assert.Equal(new IndividualId("ind_4"), result.Individual4Id);
        Assert.Equal(new IndividualId("ind_5"), result.Individual5Id);
        Assert.Equal(new IndividualId("ind_6"), result.Individual6Id);
    }

    [Fact]
    public void 個体以外のプロパティが保持される()
    {
        // Arrange
        var party = CreateParty();

        // Act
        var result = party.SetIndividuals(
            new IndividualId("ind_1"), null, null, null, null, null);

        // Assert
        Assert.Equal(party.Id, result.Id);
        Assert.Equal(party.Name, result.Name);
        Assert.Equal(party.Memo, result.Memo);
    }

    [Fact]
    public void 重複する個体を指定_例外が発生する()
    {
        // Arrange
        var party = CreateParty();

        // Act
        var exception = Record.Exception(() => party.SetIndividuals(
            new IndividualId("ind_1"), new IndividualId("ind_1"), null, null, null, null));

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void 全スロットにnullを指定_空パーティになる()
    {
        // Arrange
        var party = CreateParty();

        // Act
        var result = party.SetIndividuals(null, null, null, null, null, null);

        // Assert
        Assert.Null(result.Individual1Id);
        Assert.Null(result.Individual2Id);
        Assert.Null(result.Individual3Id);
        Assert.Null(result.Individual4Id);
        Assert.Null(result.Individual5Id);
        Assert.Null(result.Individual6Id);
    }

    private static Party CreateParty()
    {
        return new Party(
            new PartyId("pty_test"),
            "テストパーティ",
            null, null, null, null, null, null,
            "メモ"
        );
    }
}
