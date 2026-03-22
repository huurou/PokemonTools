namespace PokemonTools.Web.Domain.Items;

/// <summary>
/// 道具を表現するクラス
/// </summary>
/// <param name="Id">道具Id</param>
/// <param name="Name">道具名</param>
/// <param name="FlingPower">投げつけるの威力 投げつけるが失敗する道具はnull</param>
public record Item(ItemId Id, string Name, uint? FlingPower);
