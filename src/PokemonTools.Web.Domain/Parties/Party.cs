using PokemonTools.Web.Domain.Individuals;

namespace PokemonTools.Web.Domain.Parties;

/// <summary>
/// パーティを表現するクラス
/// </summary>
public record Party
{
    /// <summary>
    /// パーティId
    /// </summary>
    public PartyId Id { get; init; }

    /// <summary>
    /// パーティ名
    /// </summary>
    public string Name
    {
        get;
        init
        {
            ValidateName(value);
            field = value;
        }
    }

    /// <summary>
    /// 個体1Id
    /// </summary>
    public IndividualId? Individual1Id { get; init; }
    /// <summary>
    /// 個体2Id
    /// </summary>
    public IndividualId? Individual2Id { get; init; }
    /// <summary>
    /// 個体3Id
    /// </summary>
    public IndividualId? Individual3Id { get; init; }
    /// <summary>
    /// 個体4Id
    /// </summary>
    public IndividualId? Individual4Id { get; init; }
    /// <summary>
    /// 個体5Id
    /// </summary>
    public IndividualId? Individual5Id { get; init; }
    /// <summary>
    /// 個体6Id
    /// </summary>
    public IndividualId? Individual6Id { get; init; }

    /// <summary>
    /// 備考
    /// </summary>
    public string? Memo { get; init; }

    public Party(
        PartyId id,
        string name,
        IndividualId? individual1Id,
        IndividualId? individual2Id,
        IndividualId? individual3Id,
        IndividualId? individual4Id,
        IndividualId? individual5Id,
        IndividualId? individual6Id,
        string? memo
    )
    {
        Id = id;
        Name = name;
        Individual1Id = individual1Id;
        Individual2Id = individual2Id;
        Individual3Id = individual3Id;
        Individual4Id = individual4Id;
        Individual5Id = individual5Id;
        Individual6Id = individual6Id;
        Memo = memo;
    }

    private static void ValidateName(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
    }
}
