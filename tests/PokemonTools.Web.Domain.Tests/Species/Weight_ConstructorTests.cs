using PokemonTools.Web.Domain.Species;

namespace PokemonTools.Web.Domain.Tests.Species;

public class Weight_ConstructorTests
{
    [Fact]
    public void 正の値を指定_Weightが生成される()
    {
        // Arrange & Act
        var weight = new Weight(100);

        // Assert
        Assert.Equal(100, weight.Hectograms);
        Assert.Equal(10.0, weight.Kilograms);
    }

    [Fact]
    public void ゼロを指定_例外が発生する()
    {
        // Act
        var exception = Record.Exception(() => new Weight(0));

        // Assert
        Assert.IsType<ArgumentOutOfRangeException>(exception);
    }

    [Fact]
    public void 負の値を指定_例外が発生する()
    {
        // Act
        var exception = Record.Exception(() => new Weight(-1));

        // Assert
        Assert.IsType<ArgumentOutOfRangeException>(exception);
    }
}
