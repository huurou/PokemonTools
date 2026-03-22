using PokemonTools.Web.Domain.Moves;
using PokemonTools.Web.Domain.Types;

namespace PokemonTools.Web.Domain.Tests.Moves;

public class Move_ConstructorTests
{
    [Fact]
    public void 正常なパラメータ_Moveが生成される()
    {
        // Act
        var move = CreateWithDefaults();

        // Assert
        Assert.Equal(new MoveId(89), move.Id);
        Assert.Equal("じしん", move.Name);
        Assert.Equal(PokemonType.Ground.Id, move.TypeId);
        Assert.Equal(MoveDamageClass.Physical.Id, move.DamageClassId);
        Assert.Equal(100u, move.Power);
    }

    [Fact]
    public void へんか技でPowerがnull_Moveが生成される()
    {
        // Act
        var move = CreateWithDefaults(
            damageClassId: MoveDamageClass.Status.Id,
            power: null
        );

        // Assert
        Assert.Null(move.Power);
    }

    [Fact]
    public void 無効なタイプIDを指定_例外が発生する()
    {
        // Act
        var exception = Record.Exception(() => CreateWithDefaults(typeId: new TypeId(999)));

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void タイプにステラを指定_例外が発生する()
    {
        // Act
        var exception = Record.Exception(() => CreateWithDefaults(typeId: PokemonType.Stellar.Id));

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void 無効な技分類IDを指定_例外が発生する()
    {
        // Act
        var exception = Record.Exception(() => CreateWithDefaults(damageClassId: new MoveDamageClassId(999)));

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }

    private static Move CreateWithDefaults(
        MoveId? id = null,
        string name = "じしん",
        TypeId? typeId = null,
        MoveDamageClassId? damageClassId = null,
        uint? power = 100
    )
    {
        return new Move(
            id ?? new MoveId(89),
            name,
            typeId ?? PokemonType.Ground.Id,
            damageClassId ?? MoveDamageClass.Physical.Id,
            power
        );
    }
}
