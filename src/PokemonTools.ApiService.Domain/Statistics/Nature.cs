namespace PokemonTools.ApiService.Domain.Statistics;

/// <summary>
/// 性格を表現するクラス
/// </summary>
/// <param name="Id">性格Id</param>
/// <param name="Name">性格の日本語名</param>
/// <param name="IncreasedStat">上昇補正のかかる能力値</param>
/// <param name="DecreasedStat">下降補正のかかる能力値</param>
public record Nature(NatureId Id, string Name, StatType? IncreasedStat, StatType? DecreasedStat)
{
    #region シングルトンプロパティ

    /// <summary>
    /// がんばりや （補正なし）
    /// </summary>
    public static Nature Hardy { get; } = new(new NatureId("hardy"), "がんばりや", null, null);

    /// <summary>
    /// さみしがり 攻撃↑ 防御↓
    /// </summary>
    public static Nature Lonely { get; } = new(new NatureId("lonely"), "さみしがり", StatType.Attack, StatType.Defense);

    /// <summary>
    /// いじっぱり 攻撃↑ 特攻↓
    /// </summary>
    public static Nature Adamant { get; } = new(new NatureId("adamant"), "いじっぱり", StatType.Attack, StatType.SpecialAttack);

    /// <summary>
    /// やんちゃ 攻撃↑ 特防↓
    /// </summary>
    public static Nature Naughty { get; } = new(new NatureId("naughty"), "やんちゃ", StatType.Attack, StatType.SpecialDefense);

    /// <summary>
    /// ゆうかん 攻撃↑ 素早さ↓
    /// </summary>
    public static Nature Brave { get; } = new(new NatureId("brave"), "ゆうかん", StatType.Attack, StatType.Speed);

    /// <summary>
    /// ずぶとい 防御↑ 攻撃↓
    /// </summary>
    public static Nature Bold { get; } = new(new NatureId("bold"), "ずぶとい", StatType.Defense, StatType.Attack);

    /// <summary>
    /// すなお （補正なし）
    /// </summary>
    public static Nature Docile { get; } = new(new NatureId("docile"), "すなお", null, null);

    /// <summary>
    /// わんぱく 防御↑ 特攻↓
    /// </summary>
    public static Nature Impish { get; } = new(new NatureId("impish"), "わんぱく", StatType.Defense, StatType.SpecialAttack);

    /// <summary>
    /// のうてんき 防御↑ 特防↓
    /// </summary>
    public static Nature Lax { get; } = new(new NatureId("lax"), "のうてんき", StatType.Defense, StatType.SpecialDefense);

    /// <summary>
    /// のんき 防御↑ 素早さ↓
    /// </summary>
    public static Nature Relaxed { get; } = new(new NatureId("relaxed"), "のんき", StatType.Defense, StatType.Speed);

    /// <summary>
    /// ひかえめ 特攻↑ 攻撃↓
    /// </summary>
    public static Nature Modest { get; } = new(new NatureId("modest"), "ひかえめ", StatType.SpecialAttack, StatType.Attack);

    /// <summary>
    /// おっとり 特攻↑ 防御↓
    /// </summary>
    public static Nature Mild { get; } = new(new NatureId("mild"), "おっとり", StatType.SpecialAttack, StatType.Defense);

    /// <summary>
    /// てれや （補正なし）
    /// </summary>
    public static Nature Bashful { get; } = new(new NatureId("bashful"), "てれや", null, null);

    /// <summary>
    /// うっかりや 特攻↑ 特防↓
    /// </summary>
    public static Nature Rash { get; } = new(new NatureId("rash"), "うっかりや", StatType.SpecialAttack, StatType.SpecialDefense);

    /// <summary>
    /// れいせい 特攻↑ 素早さ↓
    /// </summary>
    public static Nature Quiet { get; } = new(new NatureId("quiet"), "れいせい", StatType.SpecialAttack, StatType.Speed);

    /// <summary>
    /// おだやか 特防↑ 攻撃↓
    /// </summary>
    public static Nature Calm { get; } = new(new NatureId("calm"), "おだやか", StatType.SpecialDefense, StatType.Attack);

    /// <summary>
    /// おとなしい 特防↑ 防御↓
    /// </summary>
    public static Nature Gentle { get; } = new(new NatureId("gentle"), "おとなしい", StatType.SpecialDefense, StatType.Defense);

    /// <summary>
    /// しんちょう 特防↑ 特攻↓
    /// </summary>
    public static Nature Careful { get; } = new(new NatureId("careful"), "しんちょう", StatType.SpecialDefense, StatType.SpecialAttack);

    /// <summary>
    /// なまいき 特防↑ 素早さ↓
    /// </summary>
    public static Nature Sassy { get; } = new(new NatureId("sassy"), "なまいき", StatType.SpecialDefense, StatType.Speed);

    /// <summary>
    /// おくびょう 素早さ↑ 攻撃↓
    /// </summary>
    public static Nature Timid { get; } = new(new NatureId("timid"), "おくびょう", StatType.Speed, StatType.Attack);

    /// <summary>
    /// せっかち 素早さ↑ 防御↓
    /// </summary>
    public static Nature Hasty { get; } = new(new NatureId("hasty"), "せっかち", StatType.Speed, StatType.Defense);

    /// <summary>
    /// ようき 素早さ↑ 特攻↓
    /// </summary>
    public static Nature Jolly { get; } = new(new NatureId("jolly"), "ようき", StatType.Speed, StatType.SpecialAttack);

    /// <summary>
    /// むじゃき 素早さ↑ 特防↓
    /// </summary>
    public static Nature Naive { get; } = new(new NatureId("naive"), "むじゃき", StatType.Speed, StatType.SpecialDefense);

    /// <summary>
    /// まじめ （補正なし）
    /// </summary>
    public static Nature Serious { get; } = new(new NatureId("serious"), "まじめ", null, null);

    /// <summary>
    /// きまぐれ （補正なし）
    /// </summary>
    public static Nature Quirky { get; } = new(new NatureId("quirky"), "きまぐれ", null, null);

    /// <summary>
    /// 全25性格のリスト
    /// </summary>
    public static IReadOnlyList<Nature> All { get; } =
    [
        Hardy, Lonely, Adamant, Naughty, Brave,
        Bold, Docile, Impish, Lax, Relaxed,
        Modest, Mild, Bashful, Rash, Quiet,
        Calm, Gentle, Careful, Sassy,
        Timid, Hasty, Jolly, Naive, Serious, Quirky,
    ];

    #endregion シングルトンプロパティ

    /// <summary>
    /// 指定した能力値に対する性格補正倍率を取得する
    /// </summary>
    public double GetMultiplier(StatType statType)
    {
        return IncreasedStat.HasValue && statType == IncreasedStat.Value ? 1.1
            : DecreasedStat.HasValue && statType == DecreasedStat.Value ? 0.9
            : 1.0;
    }
}
