using PokemonTools.Web.Domain.Individuals;

namespace PokemonTools.Web.Domain.Parties;

/// <summary>
/// パーティを表現するクラス
/// </summary>
/// <param name="Id">パーティId</param>
/// <param name="Name">パーティ名</param>
/// <param name="Individual1Id">個体1Id</param>
/// <param name="Individual2Id">個体2Id</param>
/// <param name="Individual3Id">個体3Id</param>
/// <param name="Individual4Id">個体4Id</param>
/// <param name="Individual5Id">個体5Id</param>
/// <param name="Individual6Id">個体6Id</param>
/// <param name="Memo">備考</param>
public record Party(
    PartyId Id,
    string Name,
    IndividualId? Individual1Id,
    IndividualId? Individual2Id,
    IndividualId? Individual3Id,
    IndividualId? Individual4Id,
    IndividualId? Individual5Id,
    IndividualId? Individual6Id,
    string? Memo
);
