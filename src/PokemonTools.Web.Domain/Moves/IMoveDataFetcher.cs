namespace PokemonTools.Web.Domain.Moves;

public interface IMoveDataFetcher
{
    /// <summary>
    /// 全技の件数を取得します。
    /// </summary>
    Task<int> GetCountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 全技を取得します。
    /// </summary>
    /// <param name="cancellationToken">キャンセルトークン</param>
    /// <returns>全技</returns>
    IAsyncEnumerable<Move> GetAllAsync(CancellationToken cancellationToken = default);
}
