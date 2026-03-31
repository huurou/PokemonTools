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

    [Fact]
    public void Nameが空文字列_例外が発生する()
    {
        // Act
        var ex = Record.Exception(() => CreateWithDefaults(name: ""));

        // Assert
        Assert.IsType<ArgumentException>(ex);
    }

    [Fact]
    public void Nameがnull_例外が発生する()
    {
        // Act
        var ex = Record.Exception(() => CreateWithDefaults(name: null!));

        // Assert
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public void Nameが空白文字のみ_例外が発生する()
    {
        // Act
        var ex = Record.Exception(() => CreateWithDefaults(name: "   "));

        // Assert
        Assert.IsType<ArgumentException>(ex);
    }

    [Fact]
    public void 個体1Idと個体2Idが同じ_例外が発生する()
    {
        // Act
        var ex = Record.Exception(() => CreateWithDefaults(
            individual1Id: new IndividualId("ind_1"),
            individual2Id: new IndividualId("ind_1")
        ));

        // Assert
        Assert.IsType<ArgumentException>(ex);
    }

    [Fact]
    public void 個体3Idと個体5Idが同じ_例外が発生する()
    {
        // Act
        var ex = Record.Exception(() => CreateWithDefaults(
            individual3Id: new IndividualId("ind_3"),
            individual5Id: new IndividualId("ind_3")
        ));

        // Assert
        Assert.IsType<ArgumentException>(ex);
    }

    [Fact]
    public void Nullのスロット同士は重複とみなさない_Partyが生成される()
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

    [Fact]
    public void 全スロットに異なる個体を指定_Partyが生成される()
    {
        // Act
        var party = CreateWithDefaults(
            individual1Id: new IndividualId("ind_1"),
            individual2Id: new IndividualId("ind_2"),
            individual3Id: new IndividualId("ind_3"),
            individual4Id: new IndividualId("ind_4"),
            individual5Id: new IndividualId("ind_5"),
            individual6Id: new IndividualId("ind_6")
        );

        // Assert
        Assert.Equal(new IndividualId("ind_1"), party.Individual1Id);
        Assert.Equal(new IndividualId("ind_2"), party.Individual2Id);
        Assert.Equal(new IndividualId("ind_3"), party.Individual3Id);
        Assert.Equal(new IndividualId("ind_4"), party.Individual4Id);
        Assert.Equal(new IndividualId("ind_5"), party.Individual5Id);
        Assert.Equal(new IndividualId("ind_6"), party.Individual6Id);
    }

    private static Party CreateWithDefaults(
        PartyId? id = null,
        string name = "テストパーティ",
        IndividualId? individual1Id = null,
        IndividualId? individual2Id = null,
        IndividualId? individual3Id = null,
        IndividualId? individual4Id = null,
        IndividualId? individual5Id = null,
        IndividualId? individual6Id = null,
        string? memo = null
    )
    {
        return new Party(
            id ?? new PartyId("pty_test"),
            name,
            individual1Id,
            individual2Id,
            individual3Id,
            individual4Id,
            individual5Id,
            individual6Id,
            memo
        );
    }
}
