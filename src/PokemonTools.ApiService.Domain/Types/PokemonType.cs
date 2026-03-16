namespace PokemonTools.ApiService.Domain.Types;

public record PokemonType(TypeId Id, string Name)
{
    #region シングルトンプロパティ

    /// <summary>
    /// ノーマル
    /// </summary>
    public static PokemonType Normal { get; } = new PokemonType(new TypeId("normal"), "ノーマル");

    /// <summary>
    /// かくとう
    /// </summary>
    public static PokemonType Fighting { get; } = new PokemonType(new TypeId("fighting"), "かくとう");

    /// <summary>
    /// ひこう
    /// </summary>
    public static PokemonType Flying { get; } = new PokemonType(new TypeId("flying"), "ひこう");
    /// <summary>
    /// どく
    /// </summary>
    public static PokemonType Poison { get; } = new PokemonType(new TypeId("poison"), "どく");

    /// <summary>
    /// じめん
    /// </summary>
    public static PokemonType Ground { get; } = new PokemonType(new TypeId("ground"), "じめん");

    /// <summary>
    /// いわ
    /// </summary>
    public static PokemonType Rock { get; } = new PokemonType(new TypeId("rock"), "いわ");

    /// <summary>
    /// むし
    /// </summary>
    public static PokemonType Bug { get; } = new PokemonType(new TypeId("bug"), "むし");

    /// <summary>
    /// ゴースト
    /// </summary>
    public static PokemonType Ghost { get; } = new PokemonType(new TypeId("ghost"), "ゴースト");

    /// <summary>
    /// はがね
    /// </summary>
    public static PokemonType Steel { get; } = new PokemonType(new TypeId("steel"), "はがね");

    /// <summary>
    /// ほのお
    /// </summary>
    public static PokemonType Fire { get; } = new PokemonType(new TypeId("fire"), "ほのお");

    /// <summary>
    /// みず
    /// </summary>
    public static PokemonType Water { get; } = new PokemonType(new TypeId("water"), "みず");

    /// <summary>
    /// くさ
    /// </summary>
    public static PokemonType Grass { get; } = new PokemonType(new TypeId("grass"), "くさ");

    /// <summary>
    /// でんき
    /// </summary>
    public static PokemonType Electric { get; } = new PokemonType(new TypeId("electric"), "でんき");

    /// <summary>
    /// エスパー
    /// </summary>
    public static PokemonType Psychic { get; } = new PokemonType(new TypeId("psychic"), "エスパー");

    /// <summary>
    /// こおり
    /// </summary>
    public static PokemonType Ice { get; } = new PokemonType(new TypeId("ice"), "こおり");

    /// <summary>
    /// ドラゴン
    /// </summary>
    public static PokemonType Dragon { get; } = new PokemonType(new TypeId("dragon"), "ドラゴン");

    /// <summary>
    /// あく
    /// </summary>
    public static PokemonType Dark { get; } = new PokemonType(new TypeId("dark"), "あく");

    /// <summary>
    /// フェアリー
    /// </summary>
    public static PokemonType Fairy { get; } = new PokemonType(new TypeId("fairy"), "フェアリー");

    #endregion シングルトンプロパティ
}
