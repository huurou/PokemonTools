namespace PokemonTools.Web.Domain.Items;

public interface IItemDataFetcher
{
    /// <summary>
    /// 全道具の件数を取得します。
    /// </summary>
    Task<int> GetCountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 全道具を取得します。
    /// </summary>
    /// <param name="cancellationToken">キャンセルトークン</param>
    /// <returns>全道具</returns>
    IAsyncEnumerable<Item> GetAllAsync(CancellationToken cancellationToken = default);
}
