namespace PokemonTools.Web.Domain.Species;

/// <summary>
/// 体重を表現するクラス
/// </summary>
public record Weight
{
    /// <summary>
    /// 体重（ヘクトグラム単位 PokeAPI生値）
    /// </summary>
    public int Hectograms
    {
        get;
        init
        {
            ValidateHectograms(value);
            field = value;
        }
    }

    /// <summary>
    /// 体重（キログラム単位 表示用）
    /// </summary>
    public double Kilograms => Hectograms / 10.0;

    public Weight(int hectograms)
    {
        Hectograms = hectograms;
    }

    private static void ValidateHectograms(int value)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);
    }
}
