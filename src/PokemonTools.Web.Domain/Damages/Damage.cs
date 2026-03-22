using System.Collections.Immutable;

namespace PokemonTools.Web.Domain.Damages;

/// <summary>
/// ダメージを表現するクラス
/// </summary>
public record Damage
{
    /// <summary>
    /// 乱数それぞれのダメージ 順に0.85,0.86,...0.99,1.00の値
    /// </summary>
    public ImmutableArray<uint> Values
    {
        get;
        init
        {
            ValidateValues(value);
            field = value;
        }
    }

    public Damage(ImmutableArray<uint> values)
    {
        Values = values;
    }

    private static void ValidateValues(ImmutableArray<uint> value)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(value.Length, 16, nameof(Values));
    }
}
