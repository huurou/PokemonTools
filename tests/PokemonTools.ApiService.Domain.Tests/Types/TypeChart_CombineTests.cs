using PokemonTools.ApiService.Domain.Types;

namespace PokemonTools.ApiService.Domain.Tests.Types;

public class TypeChart_CombineTests
{
    private static readonly TypeEffectiveness X = TypeEffectiveness.HasNoEffect;
    private static readonly TypeEffectiveness MI = TypeEffectiveness.MostlyIneffective;
    private static readonly TypeEffectiveness H = TypeEffectiveness.NotVeryEffective;
    private static readonly TypeEffectiveness N = TypeEffectiveness.Neutral;
    private static readonly TypeEffectiveness S = TypeEffectiveness.SuperEffective;
    private static readonly TypeEffectiveness EE = TypeEffectiveness.ExtremelyEffective;

    [Theory]
    [MemberData(nameof(有効な組み合わせデータ))]
    public void 有効な組み合わせ_正しい結果が返る(TypeEffectiveness e1, TypeEffectiveness e2, TypeEffectiveness expected)
    {
        // Act
        var result = TypeChart.Combine(e1, e2);

        // Assert
        Assert.Equal(expected, result);
    }

    public static TheoryData<TypeEffectiveness, TypeEffectiveness, TypeEffectiveness> 有効な組み合わせデータ => new()
    {
        // HasNoEffect × 全有効値
        { X, X, X },
        { X, H, X },
        { X, N, X },
        { X, S, X },

        // NotVeryEffective × 全有効値
        { H, X, X },
        { H, H, MI },
        { H, N, H },
        { H, S, N },

        // Neutral × 全有効値
        { N, X, X },
        { N, H, H },
        { N, N, N },
        { N, S, S },

        // SuperEffective × 全有効値
        { S, X, X },
        { S, H, N },
        { S, N, S },
        { S, S, EE },
    };

    [Theory]
    [MemberData(nameof(無効な組み合わせデータ))]
    public void 無効な組み合わせ_InvalidOperationExceptionがスローされる(TypeEffectiveness e1, TypeEffectiveness e2)
    {
        // Act
        var ex = Record.Exception(() => TypeChart.Combine(e1, e2));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
    }

    public static TheoryData<TypeEffectiveness, TypeEffectiveness> 無効な組み合わせデータ => new()
    {
        // MostlyIneffective/ExtremelyEffective は Combine の出力であり入力にはならない
        // effectiveness1 が無効
        { MI, X },
        { EE, X },

        // effectiveness2 が無効 (effectiveness1 が HasNoEffect)
        { X, MI },
        { X, EE },

        // effectiveness2 が無効 (effectiveness1 が NotVeryEffective)
        { H, MI },
        { H, EE },

        // effectiveness2 が無効 (effectiveness1 が Neutral)
        { N, MI },
        { N, EE },

        // effectiveness2 が無効 (effectiveness1 が SuperEffective)
        { S, MI },
        { S, EE },

        // 両方が無効値
        { MI, MI },
        { MI, EE },
        { EE, MI },
        { EE, EE },
    };
}
