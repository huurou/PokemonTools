namespace PokemonTools.Web.Domain.Abilities;

public interface IAbilityDataFetcher
{
    /// <summary>
    /// 全特性の件数を取得します。
    /// </summary>
    Task<int> GetCountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 全特性を取得します。
    /// </summary>
    /// <param name="cancellationToken">キャンセルトークン</param>
    /// <returns>全特性</returns>
    IAsyncEnumerable<Ability> GetAllAsync(CancellationToken cancellationToken = default);
}
