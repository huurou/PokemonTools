using System.Collections.Immutable;

namespace PokemonTools.Web.Domain.Statistics;

/// <summary>
/// 能力補正を表現するクラス
/// </summary>
/// <param name="Id">能力補正Id</param>
/// <param name="Name">能力補正の日本語名</param>
/// <param name="IncreasedStat">上昇補正のかかる能力値</param>
/// <param name="DecreasedStat">下降補正のかかる能力値</param>
public record StatAlignment(StatAlignmentId Id, string Name, StatType? IncreasedStat, StatType? DecreasedStat)
{
    #region シングルトンプロパティ

    /// <summary>
    /// がんばりや （補正なし）
    /// </summary>
    public static StatAlignment Hardy { get; } = new(new StatAlignmentId(1), "がんばりや", null, null);

    /// <summary>
    /// ずぶとい 防御↑ 攻撃↓
    /// </summary>
    public static StatAlignment Bold { get; } = new(new StatAlignmentId(2), "ずぶとい", StatType.Defense, StatType.Attack);

    /// <summary>
    /// ひかえめ 特攻↑ 攻撃↓
    /// </summary>
    public static StatAlignment Modest { get; } = new(new StatAlignmentId(3), "ひかえめ", StatType.SpecialAttack, StatType.Attack);

    /// <summary>
    /// おだやか 特防↑ 攻撃↓
    /// </summary>
    public static StatAlignment Calm { get; } = new(new StatAlignmentId(4), "おだやか", StatType.SpecialDefense, StatType.Attack);

    /// <summary>
    /// おくびょう 素早さ↑ 攻撃↓
    /// </summary>
    public static StatAlignment Timid { get; } = new(new StatAlignmentId(5), "おくびょう", StatType.Speed, StatType.Attack);

    /// <summary>
    /// さみしがり 攻撃↑ 防御↓
    /// </summary>
    public static StatAlignment Lonely { get; } = new(new StatAlignmentId(6), "さみしがり", StatType.Attack, StatType.Defense);

    /// <summary>
    /// すなお （補正なし）
    /// </summary>
    public static StatAlignment Docile { get; } = new(new StatAlignmentId(7), "すなお", null, null);

    /// <summary>
    /// おっとり 特攻↑ 防御↓
    /// </summary>
    public static StatAlignment Mild { get; } = new(new StatAlignmentId(8), "おっとり", StatType.SpecialAttack, StatType.Defense);

    /// <summary>
    /// おとなしい 特防↑ 防御↓
    /// </summary>
    public static StatAlignment Gentle { get; } = new(new StatAlignmentId(9), "おとなしい", StatType.SpecialDefense, StatType.Defense);

    /// <summary>
    /// せっかち 素早さ↑ 防御↓
    /// </summary>
    public static StatAlignment Hasty { get; } = new(new StatAlignmentId(10), "せっかち", StatType.Speed, StatType.Defense);

    /// <summary>
    /// いじっぱり 攻撃↑ 特攻↓
    /// </summary>
    public static StatAlignment Adamant { get; } = new(new StatAlignmentId(11), "いじっぱり", StatType.Attack, StatType.SpecialAttack);

    /// <summary>
    /// わんぱく 防御↑ 特攻↓
    /// </summary>
    public static StatAlignment Impish { get; } = new(new StatAlignmentId(12), "わんぱく", StatType.Defense, StatType.SpecialAttack);

    /// <summary>
    /// てれや （補正なし）
    /// </summary>
    public static StatAlignment Bashful { get; } = new(new StatAlignmentId(13), "てれや", null, null);

    /// <summary>
    /// しんちょう 特防↑ 特攻↓
    /// </summary>
    public static StatAlignment Careful { get; } = new(new StatAlignmentId(14), "しんちょう", StatType.SpecialDefense, StatType.SpecialAttack);

    /// <summary>
    /// うっかりや 特攻↑ 特防↓
    /// </summary>
    public static StatAlignment Rash { get; } = new(new StatAlignmentId(15), "うっかりや", StatType.SpecialAttack, StatType.SpecialDefense);

    /// <summary>
    /// ようき 素早さ↑ 特攻↓
    /// </summary>
    public static StatAlignment Jolly { get; } = new(new StatAlignmentId(16), "ようき", StatType.Speed, StatType.SpecialAttack);

    /// <summary>
    /// やんちゃ 攻撃↑ 特防↓
    /// </summary>
    public static StatAlignment Naughty { get; } = new(new StatAlignmentId(17), "やんちゃ", StatType.Attack, StatType.SpecialDefense);

    /// <summary>
    /// のうてんき 防御↑ 特防↓
    /// </summary>
    public static StatAlignment Lax { get; } = new(new StatAlignmentId(18), "のうてんき", StatType.Defense, StatType.SpecialDefense);

    /// <summary>
    /// きまぐれ （補正なし）
    /// </summary>
    public static StatAlignment Quirky { get; } = new(new StatAlignmentId(19), "きまぐれ", null, null);

    /// <summary>
    /// むじゃき 素早さ↑ 特防↓
    /// </summary>
    public static StatAlignment Naive { get; } = new(new StatAlignmentId(20), "むじゃき", StatType.Speed, StatType.SpecialDefense);

    /// <summary>
    /// ゆうかん 攻撃↑ 素早さ↓
    /// </summary>
    public static StatAlignment Brave { get; } = new(new StatAlignmentId(21), "ゆうかん", StatType.Attack, StatType.Speed);

    /// <summary>
    /// のんき 防御↑ 素早さ↓
    /// </summary>
    public static StatAlignment Relaxed { get; } = new(new StatAlignmentId(22), "のんき", StatType.Defense, StatType.Speed);

    /// <summary>
    /// れいせい 特攻↑ 素早さ↓
    /// </summary>
    public static StatAlignment Quiet { get; } = new(new StatAlignmentId(23), "れいせい", StatType.SpecialAttack, StatType.Speed);

    /// <summary>
    /// なまいき 特防↑ 素早さ↓
    /// </summary>
    public static StatAlignment Sassy { get; } = new(new StatAlignmentId(24), "なまいき", StatType.SpecialDefense, StatType.Speed);

    /// <summary>
    /// まじめ （補正なし）
    /// </summary>
    public static StatAlignment Serious { get; } = new(new StatAlignmentId(25), "まじめ", null, null);

    /// <summary>
    /// 全25能力補正のリスト
    /// </summary>
    public static ImmutableArray<StatAlignment> All { get; } =
    [
        Hardy, Bold, Modest, Calm, Timid,
        Lonely, Docile, Mild, Gentle, Hasty,
        Adamant, Impish, Bashful, Careful, Rash,
        Jolly, Naughty, Lax, Quirky, Naive,
        Brave, Relaxed, Quiet, Sassy, Serious,
    ];

    #endregion シングルトンプロパティ

    /// <summary>
    /// 指定した能力値に対する能力補正倍率を取得する
    /// </summary>
    public double GetMultiplier(StatType statType)
    {
        return IncreasedStat.HasValue && statType == IncreasedStat.Value ? 1.1
            : DecreasedStat.HasValue && statType == DecreasedStat.Value ? 0.9
            : 1.0;
    }
}
