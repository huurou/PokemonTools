namespace PokemonTools.Web.Domain.Species;

/// <summary>
/// 体重を表現するクラス
/// </summary>
public record Weight
{
    /// <summary>
    /// 体重（ヘクトグラム単位 PokeAPI生値）
    /// </summary>
    public int Hectograms { get; init; }

    /// <summary>
    /// 体重（キログラム単位 表示用）
    /// </summary>
    public double Kilograms => Hectograms / 10.0;

    public Weight(int hectograms)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(hectograms);

        Hectograms = hectograms;
    }
}
