using PokemonTools.Web.Domain.Individuals;
using PokemonTools.Web.Domain.Parties;

namespace PokemonTools.Web.Domain.Tests.Parties;

public class Party_ConstructorTests
{
    [Fact]
    public void 正常なパラメータ_Partyが生成される()
    {
        // Act
        var party = CreateWithDefaults(individual1Id: new IndividualId("ind_1"));

        // Assert
        Assert.Equal(new PartyId("pty_test"), party.Id);
        Assert.Equal("テストパーティ", party.Name);
        Assert.Equal(new IndividualId("ind_1"), party.Individual1Id);
    }

    [Fact]
    public void 全スロットがnull_空パーティが生成される()
    {
        // Act
        var party = CreateWithDefaults();

        // Assert
        Assert.Null(party.Individual1Id);
        Assert.Null(party.Individual2Id);
        Assert.Null(party.Individual3Id);
        Assert.Null(party.Individual4Id);
        Assert.Null(party.Individual5Id);
        Assert.Null(party.Individual6Id);
    }

    private static Party CreateWithDefaults(
        PartyId? id = null,
        string name = "テストパーティ",
        IndividualId? individual1Id = null,
        string? memo = null
    )
    {
        return new Party(
            id ?? new PartyId("pty_test"),
            name,
            individual1Id,
            Individual2Id: null,
            Individual3Id: null,
            Individual4Id: null,
            Individual5Id: null,
            Individual6Id: null,
            memo
        );
    }
}
