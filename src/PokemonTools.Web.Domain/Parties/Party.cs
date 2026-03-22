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
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            field = value;
        }
    }

    // Individual1Id~Individual6Idは組み合わせで不変条件を持つため、with式での部分更新を防ぐ。
    // 個体変更はSetIndividuals経由で行う。

    /// <summary>
    /// 個体1Id
    /// </summary>
    public IndividualId? Individual1Id { get; private init; }
    /// <summary>
    /// 個体2Id
    /// </summary>
    public IndividualId? Individual2Id { get; private init; }
    /// <summary>
    /// 個体3Id
    /// </summary>
    public IndividualId? Individual3Id { get; private init; }
    /// <summary>
    /// 個体4Id
    /// </summary>
    public IndividualId? Individual4Id { get; private init; }
    /// <summary>
    /// 個体5Id
    /// </summary>
    public IndividualId? Individual5Id { get; private init; }
    /// <summary>
    /// 個体6Id
    /// </summary>
    public IndividualId? Individual6Id { get; private init; }

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

        IndividualId?[] individualIds = [Individual1Id, Individual2Id, Individual3Id, Individual4Id, Individual5Id, Individual6Id];
        var nonNullIndividualIds = individualIds.Where(x => x is not null).ToArray();
        if (nonNullIndividualIds.Length != nonNullIndividualIds.Distinct().Count())
        {
            throw new ArgumentException("同じ個体を複数スロットに設定することはできません。");
        }

        Memo = memo;
    }

    public Party SetIndividuals(
        IndividualId? individual1Id, IndividualId? individual2Id, IndividualId? individual3Id,
        IndividualId? individual4Id, IndividualId? individual5Id, IndividualId? individual6Id)
    {
        return new Party(Id, Name, individual1Id, individual2Id, individual3Id,
            individual4Id, individual5Id, individual6Id, Memo);
    }
}
