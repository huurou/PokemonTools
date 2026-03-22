using System.Collections.Immutable;

namespace PokemonTools.Web.Domain.Individuals;

/// <summary>
/// 個体のカテゴリを表現するクラス
/// </summary>
/// <param name="Id">個体カテゴリId</param>
/// <param name="Name">個体カテゴリの日本語名</param>
public record IndividualCategory(IndividualCategoryId Id, string Name)
{
    #region シングルトンプロパティ

    /// <summary>
    /// 手持ち個体
    /// </summary>
    public static IndividualCategory OwnedIndividual { get; } = new(new IndividualCategoryId(1), "手持ち個体");

    /// <summary>
    /// ダメージ計算プリセット個体
    /// </summary>
    public static IndividualCategory DamageCalculationPreset { get; } = new(new IndividualCategoryId(2), "ダメージ計算プリセット個体");

    #endregion シングルトンプロパティ

    /// <summary>
    /// 全2カテゴリのリスト
    /// </summary>
    public static ImmutableArray<IndividualCategory> All { get; } =
    [
        OwnedIndividual, DamageCalculationPreset,
    ];
}
