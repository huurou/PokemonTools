using PokemonTools.Web.Domain.Statistics;

namespace PokemonTools.Web.Domain.Tests.Statistics;

public class StatPoints_SetValuesTests
{
    [Fact]
    public void 一部の値を変更_指定した値のみ変更される()
    {
        // Arrange
        var sp = new StatPoints(0, 32, 0, 0, 2, 32);

        // Act
        var result = sp.SetValues(hp: 32, attack: 0);

        // Assert
        Assert.Equal(32u, result.Hp);
        Assert.Equal(0u, result.Attack);
        Assert.Equal(0u, result.Defense);
        Assert.Equal(0u, result.SpecialAttack);
        Assert.Equal(2u, result.SpecialDefense);
        Assert.Equal(32u, result.Speed);
    }

    [Fact]
    public void 引数なし_元の値がそのまま返る()
    {
        // Arrange
        var sp = new StatPoints(0, 32, 0, 0, 2, 32);

        // Act
        var result = sp.SetValues();

        // Assert
        Assert.Equal(sp, result);
    }

    [Fact]
    public void 合計が66を超える値に変更_例外が発生する()
    {
        // Arrange
        var sp = new StatPoints(0, 32, 0, 0, 2, 32);

        // Act
        var exception = Record.Exception(() => sp.SetValues(hp: 32));

        // Assert
        Assert.IsType<ArgumentOutOfRangeException>(exception);
    }

    [Fact]
    public void 個別の値が32を超える値に変更_例外が発生する()
    {
        // Arrange
        var sp = new StatPoints(0, 0, 0, 0, 0, 0);

        // Act
        var exception = Record.Exception(() => sp.SetValues(hp: 33));

        // Assert
        Assert.IsType<ArgumentOutOfRangeException>(exception);
    }

    [Fact]
    public void 合計ちょうど66_正常に生成される()
    {
        // Arrange
        var sp = new StatPoints(0, 0, 0, 0, 0, 0);

        // Act
        var result = sp.SetValues(hp: 11, attack: 11, defense: 11, specialAttack: 11, specialDefense: 11, speed: 11);

        // Assert
        Assert.Equal(11u, result.Hp);
        Assert.Equal(11u, result.Attack);
        Assert.Equal(11u, result.Defense);
        Assert.Equal(11u, result.SpecialAttack);
        Assert.Equal(11u, result.SpecialDefense);
        Assert.Equal(11u, result.Speed);
    }

    [Fact]
    public void 個別の値がちょうど32_正常に生成される()
    {
        // Arrange
        var sp = new StatPoints(0, 0, 0, 0, 0, 0);

        // Act
        var result = sp.SetValues(hp: 32, attack: 32, defense: 2);

        // Assert
        Assert.Equal(32u, result.Hp);
        Assert.Equal(32u, result.Attack);
        Assert.Equal(2u, result.Defense);
    }
}
