using PokemonTools.Web.Domain.Statistics;

namespace PokemonTools.Web.Domain.Tests.Statistics;

public class EffortValues_SetValuesTests
{
    [Fact]
    public void 一部の値を変更_指定した値のみ変更される()
    {
        // Arrange
        var ev = new EffortValues(0, 252, 0, 0, 4, 252);

        // Act
        var result = ev.SetValues(hp: 252, attack: 0);

        // Assert
        Assert.Equal(252u, result.Hp);
        Assert.Equal(0u, result.Attack);
        Assert.Equal(0u, result.Defense);
        Assert.Equal(0u, result.SpecialAttack);
        Assert.Equal(4u, result.SpecialDefense);
        Assert.Equal(252u, result.Speed);
    }

    [Fact]
    public void 引数なし_元の値がそのまま返る()
    {
        // Arrange
        var ev = new EffortValues(0, 252, 0, 0, 4, 252);

        // Act
        var result = ev.SetValues();

        // Assert
        Assert.Equal(ev, result);
    }

    [Fact]
    public void 合計が510を超える値に変更_例外が発生する()
    {
        // Arrange
        var ev = new EffortValues(0, 252, 0, 0, 4, 252);

        // Act
        var exception = Record.Exception(() => ev.SetValues(hp: 252));

        // Assert
        Assert.IsType<ArgumentOutOfRangeException>(exception);
    }

    [Fact]
    public void 個別の値が252を超える値に変更_例外が発生する()
    {
        // Arrange
        var ev = new EffortValues(0, 0, 0, 0, 0, 0);

        // Act
        var exception = Record.Exception(() => ev.SetValues(hp: 253));

        // Assert
        Assert.IsType<ArgumentOutOfRangeException>(exception);
    }
}
