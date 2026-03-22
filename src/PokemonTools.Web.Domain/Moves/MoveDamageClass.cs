using System.Collections.Immutable;

namespace PokemonTools.Web.Domain.Moves;

/// <summary>
/// 技の分類（へんか・ぶつり・とくしゅ）を表現するクラス
/// </summary>
/// <param name="Id">技分類Id</param>
/// <param name="Name">技分類の日本語名</param>
public record MoveDamageClass(MoveDamageClassId Id, string Name)
{
    #region シングルトンプロパティ

    /// <summary>
    /// へんか
    /// </summary>
    public static MoveDamageClass Status { get; } = new(new MoveDamageClassId(1), "へんか");

    /// <summary>
    /// ぶつり
    /// </summary>
    public static MoveDamageClass Physical { get; } = new(new MoveDamageClassId(2), "ぶつり");

    /// <summary>
    /// とくしゅ
    /// </summary>
    public static MoveDamageClass Special { get; } = new(new MoveDamageClassId(3), "とくしゅ");

    #endregion シングルトンプロパティ

    /// <summary>
    /// 全3分類のリスト
    /// </summary>
    public static ImmutableArray<MoveDamageClass> All { get; } =
    [
        Status, Physical, Special,
    ];
}
