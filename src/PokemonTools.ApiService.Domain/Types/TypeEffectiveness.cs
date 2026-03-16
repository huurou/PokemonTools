namespace PokemonTools.ApiService.Domain.Types;

/// <summary>
/// タイプ相性の倍率を表す列挙型
/// </summary>
public enum TypeEffectiveness
{
    /// <summary>
    /// 効果がない (0倍)
    /// </summary>
    HasNoEffect,

    /// <summary>
    /// 効果はかなり今一つ (0.25倍)
    /// </summary>
    MostlyIneffective,

    /// <summary>
    /// 効果は今一つ (0.5倍)
    /// </summary>
    NotVeryEffective,

    /// <summary>
    /// 等倍 (1倍)
    /// </summary>
    Neutral,

    /// <summary>
    /// 効果はバツグン (2倍)
    /// </summary>
    SuperEffective,

    /// <summary>
    /// 効果はちょうバツグン (4倍)
    /// </summary>
    ExtremelyEffective,
}
