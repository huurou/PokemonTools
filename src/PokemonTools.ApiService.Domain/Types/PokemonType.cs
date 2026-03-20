namespace PokemonTools.ApiService.Domain.Types;

/// <summary>
/// ポケモンのタイプを表現するクラス
/// </summary>
/// <param name="Id">タイプId</param>
/// <param name="Name">タイプの日本語名</param>
public record PokemonType(TypeId Id, string Name)
{
    #region シングルトンプロパティ

    /// <summary>
    /// ノーマル
    /// </summary>
    public static PokemonType Normal { get; } = new PokemonType(new TypeId(1), "ノーマル");

    /// <summary>
    /// かくとう
    /// </summary>
    public static PokemonType Fighting { get; } = new PokemonType(new TypeId(2), "かくとう");

    /// <summary>
    /// ひこう
    /// </summary>
    public static PokemonType Flying { get; } = new PokemonType(new TypeId(3), "ひこう");

    /// <summary>
    /// どく
    /// </summary>
    public static PokemonType Poison { get; } = new PokemonType(new TypeId(4), "どく");

    /// <summary>
    /// じめん
    /// </summary>
    public static PokemonType Ground { get; } = new PokemonType(new TypeId(5), "じめん");

    /// <summary>
    /// いわ
    /// </summary>
    public static PokemonType Rock { get; } = new PokemonType(new TypeId(6), "いわ");

    /// <summary>
    /// むし
    /// </summary>
    public static PokemonType Bug { get; } = new PokemonType(new TypeId(7), "むし");

    /// <summary>
    /// ゴースト
    /// </summary>
    public static PokemonType Ghost { get; } = new PokemonType(new TypeId(8), "ゴースト");

    /// <summary>
    /// はがね
    /// </summary>
    public static PokemonType Steel { get; } = new PokemonType(new TypeId(9), "はがね");

    /// <summary>
    /// ほのお
    /// </summary>
    public static PokemonType Fire { get; } = new PokemonType(new TypeId(10), "ほのお");

    /// <summary>
    /// みず
    /// </summary>
    public static PokemonType Water { get; } = new PokemonType(new TypeId(11), "みず");

    /// <summary>
    /// くさ
    /// </summary>
    public static PokemonType Grass { get; } = new PokemonType(new TypeId(12), "くさ");

    /// <summary>
    /// でんき
    /// </summary>
    public static PokemonType Electric { get; } = new PokemonType(new TypeId(13), "でんき");

    /// <summary>
    /// エスパー
    /// </summary>
    public static PokemonType Psychic { get; } = new PokemonType(new TypeId(14), "エスパー");

    /// <summary>
    /// こおり
    /// </summary>
    public static PokemonType Ice { get; } = new PokemonType(new TypeId(15), "こおり");

    /// <summary>
    /// ドラゴン
    /// </summary>
    public static PokemonType Dragon { get; } = new PokemonType(new TypeId(16), "ドラゴン");

    /// <summary>
    /// あく
    /// </summary>
    public static PokemonType Dark { get; } = new PokemonType(new TypeId(17), "あく");

    /// <summary>
    /// フェアリー
    /// </summary>
    public static PokemonType Fairy { get; } = new PokemonType(new TypeId(18), "フェアリー");

    /// <summary>
    /// ステラ
    /// </summary>
    public static PokemonType Stellar { get; } = new PokemonType(new TypeId(19), "ステラ");

    /// <summary>
    /// ???
    /// </summary>
    public static PokemonType Unknown { get; } = new PokemonType(new TypeId(10001), "???");

    #endregion シングルトンプロパティ

    /// <summary>
    /// 相性表が定義されている18タイプのリスト
    /// </summary>
    public static IReadOnlyList<PokemonType> BattleTypes { get; } =
    [
        Normal, Fighting, Flying, Poison, Ground, Rock, Bug, Ghost, Steel,
        Fire, Water, Grass, Electric, Psychic, Ice, Dragon, Dark, Fairy,
    ];

    /// <summary>
    /// 全20タイプのリスト（ステラ・???含む）
    /// </summary>
    public static IReadOnlyList<PokemonType> All { get; } =
    [
        Normal, Fighting, Flying, Poison, Ground, Rock, Bug, Ghost, Steel,
        Fire, Water, Grass, Electric, Psychic, Ice, Dragon, Dark, Fairy,
        Stellar, Unknown,
    ];
}
