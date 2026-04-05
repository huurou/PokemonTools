using PokemonTools.Web.Domain.Abilities;
using PokemonTools.Web.Domain.Items;
using PokemonTools.Web.Domain.Moves;
using PokemonTools.Web.Domain.Species;
using PokemonTools.Web.Domain.Statistics;
using PokemonTools.Web.Domain.Types;

namespace PokemonTools.Web.Domain.Individuals;

/// <summary>
/// 育成済みポケモン個体を表現するクラス
/// </summary>
public record Individual
{
    /// <summary>
    /// 個体Id
    /// </summary>
    public IndividualId Id { get; init; }

    /// <summary>
    /// 個体名 nullの場合は種族名で表示
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// 種族Id
    /// </summary>
    public SpeciesId SpeciesId { get; init; }

    /// <summary>
    /// 能力補正Id
    /// </summary>
    public StatAlignmentId StatAlignmentId
    {
        get;
        init
        {
            if (!StatAlignment.All.Any(x => x.Id == value))
            {
                throw new ArgumentException("無効な能力補正Idです。", nameof(StatAlignmentId));
            }
            field = value;
        }
    }

    /// <summary>
    /// 特性Id
    /// </summary>
    public AbilityId AbilityId { get; init; }

    /// <summary>
    /// 能力ポイント
    /// </summary>
    public StatPoints StatPoints { get; init; }

    // Move1Id~Move4Idは組み合わせで不変条件を持つため、with式での部分更新を防ぐ。
    // 技変更はSetMoves経由で行う。

    /// <summary>
    /// 技1Id
    /// </summary>
    public MoveId Move1Id { get; private init; }
    /// <summary>
    /// 技2Id
    /// </summary>
    public MoveId? Move2Id { get; private init; }
    /// <summary>
    /// 技3Id
    /// </summary>
    public MoveId? Move3Id { get; private init; }
    /// <summary>
    /// 技4Id
    /// </summary>
    public MoveId? Move4Id { get; private init; }

    /// <summary>
    /// 持ち物の道具Id
    /// </summary>
    public ItemId? HeldItemId { get; init; }

    /// <summary>
    /// テラスタイプId
    /// </summary>
    public TypeId TeraTypeId
    {
        get;
        init
        {
            if (!PokemonType.All.Any(x => x.Id == value) || value == PokemonType.Unknown.Id)
            {
                throw new ArgumentException("テラスタイプに???または無効なタイプは指定できません。", nameof(TeraTypeId));
            }
            field = value;
        }
    }

    /// <summary>
    /// 備考
    /// </summary>
    public string? Memo { get; init; }

    /// <summary>
    /// 個体カテゴリId
    /// </summary>
    public IndividualCategoryId CategoryId
    {
        get;
        init
        {
            if (!IndividualCategory.All.Any(x => x.Id == value))
            {
                throw new ArgumentException("無効な個体カテゴリIdです。", nameof(CategoryId));
            }
            field = value;
        }
    }

    public Individual(
        IndividualId id,
        string? name,
        SpeciesId speciesId,
        StatAlignmentId statAlignmentId,
        AbilityId abilityId,
        StatPoints statPoints,
        MoveId move1Id,
        MoveId? move2Id,
        MoveId? move3Id,
        MoveId? move4Id,
        ItemId? heldItemId,
        TypeId teraTypeId,
        string? memo,
        IndividualCategoryId categoryId
    )
    {
        Id = id;
        Name = name;
        SpeciesId = speciesId;
        StatAlignmentId = statAlignmentId;
        AbilityId = abilityId;
        StatPoints = statPoints;
        Move1Id = move1Id;
        Move2Id = move2Id;
        Move3Id = move3Id;
        Move4Id = move4Id;

        MoveId?[] moveIds = [Move1Id, Move2Id, Move3Id, Move4Id];
        var nonNullMoveIds = moveIds.Where(x => x is not null).ToArray();
        if (nonNullMoveIds.Length != nonNullMoveIds.Distinct().Count())
        {
            throw new ArgumentException("同じ技を複数スロットに設定することはできません。");
        }

        HeldItemId = heldItemId;
        TeraTypeId = teraTypeId;
        Memo = memo;
        CategoryId = categoryId;
    }

    public Individual SetMoves(MoveId move1Id, MoveId? move2Id, MoveId? move3Id, MoveId? move4Id)
    {
        return new Individual(Id, Name, SpeciesId, StatAlignmentId, AbilityId, StatPoints,
            move1Id, move2Id, move3Id, move4Id, HeldItemId, TeraTypeId, Memo, CategoryId);
    }

    public Individual Update(
        string? name,
        SpeciesId speciesId,
        StatAlignmentId statAlignmentId,
        AbilityId abilityId,
        StatPoints statPoints,
        MoveId move1Id,
        MoveId? move2Id,
        MoveId? move3Id,
        MoveId? move4Id,
        ItemId? heldItemId,
        TypeId teraTypeId,
        string? memo)
    {
        return new Individual(Id, name, speciesId, statAlignmentId, abilityId, statPoints,
            move1Id, move2Id, move3Id, move4Id, heldItemId, teraTypeId, memo, CategoryId);
    }
}
