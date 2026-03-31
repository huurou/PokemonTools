namespace PokemonTools.Web.Domain.Species;

public interface ISpeciesDataFetcher
{
    /// <summary>
    /// 全種族の件数を取得します。
    /// </summary>
    Task<int> GetCountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 全種族を取得します。
    /// </summary>
    /// <param name="cancellationToken">キャンセルトークン</param>
    /// <returns>全種族</returns>
    IAsyncEnumerable<PokemonSpecies> GetAllAsync(CancellationToken cancellationToken = default);
}
