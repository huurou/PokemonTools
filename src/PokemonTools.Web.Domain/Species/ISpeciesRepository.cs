namespace PokemonTools.Web.Domain.Species;

public interface ISpeciesRepository
{
    /// <summary>
    /// 種族を一括で更新します。
    /// </summary>
    /// <param name="species">対象種族のリスト</param>
    /// <param name="cancellationToken">キャンセルトークン</param>
    /// <returns></returns>
    Task UpsertRangeAsync(List<PokemonSpecies> species, CancellationToken cancellationToken = default);
}
