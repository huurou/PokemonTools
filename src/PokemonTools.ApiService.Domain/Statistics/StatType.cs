namespace PokemonTools.ApiService.Domain.Statistics;

/// <summary>
/// HP以外の能力値の種別
/// </summary>
public enum StatType
{
    /// <summary>
    /// こうげき
    /// </summary>
    Attack,

    /// <summary>
    /// ぼうぎょ
    /// </summary>
    Defense,

    /// <summary>
    /// とくこう
    /// </summary>
    SpecialAttack,

    /// <summary>
    /// とくぼう
    /// </summary>
    SpecialDefense,

    /// <summary>
    /// すばやさ
    /// </summary>
    Speed,
}
