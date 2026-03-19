using PokemonTools.ApiService.Domain.Statistics;

namespace PokemonTools.ApiService.Domain.Tests.Statistics;

public class NatureId_ConstructorTests
{
    [Fact]
    public void 有効な文字列_NatureIdが生成される()
    {
        // Arrange & Act
        var natureId = new NatureId("adamant");

        // Assert
        Assert.Equal("adamant", natureId.Value);
    }

    [Fact]
    public void Nullを渡す_ArgumentExceptionがスローされる()
    {
        // Arrange & Act
        var exception = Record.Exception(() => new NatureId(null!));

        // Assert
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void 空文字を渡す_ArgumentExceptionがスローされる()
    {
        // Arrange & Act
        var exception = Record.Exception(() => new NatureId(""));

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void 空白文字を渡す_ArgumentExceptionがスローされる()
    {
        // Arrange & Act
        var exception = Record.Exception(() => new NatureId(" "));

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }
}
