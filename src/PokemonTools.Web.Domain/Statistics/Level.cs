namespace PokemonTools.Web.Domain.Statistics;

/// <summary>
/// レベルを表現するクラス
/// </summary>
public record Level
{
    /// <summary>
    /// 値
    /// </summary>
    public uint Value
    {
        get;
        init
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, 1u);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 100u);
            field = value;
        }
    }

    public Level(uint value)
    {
        Value = value;
    }
}
