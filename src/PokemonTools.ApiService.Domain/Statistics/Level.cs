namespace PokemonTools.ApiService.Domain.Statistics;

/// <summary>
/// レベルを表現するクラス
/// </summary>
public record Level
{
    /// <summary>
    /// 値
    /// </summary>
    public uint Value { get; }

    public Level(uint value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, 1u);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 100u);
        Value = value;
    }
}
