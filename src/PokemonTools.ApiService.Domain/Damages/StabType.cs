namespace PokemonTools.ApiService.Domain.Damages;

/// <summary>
/// タイプ一致ボーナス(STAB)の種類を表す列挙型
/// </summary>
public enum StabType
{
    /// <summary>
    /// タイプ不一致
    /// </summary>
    None,

    /// <summary>
    /// タイプ一致 (1.5倍 = 6144/4096)
    /// </summary>
    Normal,

    /// <summary>
    /// タイプ一致+てきおうりょく (2.0倍 = 8192/4096)
    /// </summary>
    Adaptability,

    /// <summary>
    /// タイプ一致+てきおうりょく+テラスタル (2.25倍 = 9216/4096)
    /// </summary>
    AdaptabilityTerastal,
}
