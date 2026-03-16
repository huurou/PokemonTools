using System.Collections.Immutable;

namespace PokemonTools.ApiService.Domain.Damages;

/// <summary>
/// ダメージを表現するクラス
/// </summary>
public record Damage
{
    /// <summary>
    /// 乱数それぞれのダメージ 順に0.85,0.86,...0.99,1.00の値
    /// </summary>
    public ImmutableArray<uint> Values { get; }

    public Damage(ImmutableArray<uint> values)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(values.Length, 16, nameof(values));
        Values = values;
    }
}
