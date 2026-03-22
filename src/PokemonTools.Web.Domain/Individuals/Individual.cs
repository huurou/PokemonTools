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
    /// 性格Id
    /// </summary>
    public NatureId NatureId
    {
        get;
        init
        {
            ValidateNatureId(value);
            field = value;
        }
    }

    /// <summary>
    /// 特性Id
    /// </summary>
    public AbilityId AbilityId { get; init; }

    /// <summary>
    /// 個体値
    /// </summary>
    public IndividualValues IndividualValues { get; init; }

    /// <summary>
    /// 努力値
    /// </summary>
    public EffortValues EffortValues { get; init; }

    /// <summary>
    /// 技1Id
    /// </summary>
    public MoveId Move1Id { get; init; }
    /// <summary>
    /// 技2Id
    /// </summary>
    public MoveId? Move2Id { get; init; }
    /// <summary>
    /// 技3Id
    /// </summary>
    public MoveId? Move3Id { get; init; }
    /// <summary>
    /// 技4Id
    /// </summary>
    public MoveId? Move4Id { get; init; }

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
            ValidateTeraTypeId(value);
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
            ValidateCategoryId(value);
            field = value;
        }
    }

    public Individual(
        IndividualId id,
        string? name,
        SpeciesId speciesId,
        NatureId natureId,
        AbilityId abilityId,
        IndividualValues individualValues,
        EffortValues effortValues,
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
        NatureId = natureId;
        AbilityId = abilityId;
        IndividualValues = individualValues;
        EffortValues = effortValues;
        Move1Id = move1Id;
        Move2Id = move2Id;
        Move3Id = move3Id;
        Move4Id = move4Id;
        HeldItemId = heldItemId;
        TeraTypeId = teraTypeId;
        Memo = memo;
        CategoryId = categoryId;
    }

    private static void ValidateTeraTypeId(TypeId value)
    {
        if (!PokemonType.All.Any(x => x.Id == value) || value == PokemonType.Unknown.Id)
        {
            throw new ArgumentException("テラスタイプに???または無効なタイプは指定できません。", nameof(TeraTypeId));
        }
    }

    private static void ValidateNatureId(NatureId value)
    {
        if (!Nature.All.Any(x => x.Id == value))
        {
            throw new ArgumentException("無効な性格Idです。", nameof(NatureId));
        }
    }

    private static void ValidateCategoryId(IndividualCategoryId value)
    {
        if (!IndividualCategory.All.Any(x => x.Id == value))
        {
            throw new ArgumentException("無効な個体カテゴリIdです。", nameof(CategoryId));
        }
    }
}
