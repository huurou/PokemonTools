namespace PokemonTools.Web.Domain.Items;

public interface IItemRepository
{
    /// <summary>
    /// 道具を一括で更新します。
    /// </summary>
    /// <param name="items">対象道具のリスト</param>
    /// <param name="cancellationToken">キャンセルトークン</param>
    /// <returns></returns>
    Task UpsertRangeAsync(List<Item> items, CancellationToken cancellationToken = default);
}
