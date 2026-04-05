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

    /// <summary>
    /// 全種族を取得します。
    /// </summary>
    Task<List<PokemonSpecies>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 指定したIDの種族を取得します。
    /// </summary>
    Task<PokemonSpecies?> FindByIdAsync(SpeciesId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 指定した複数IDの種族を取得します。
    /// </summary>
    Task<List<PokemonSpecies>> GetByIdsAsync(IEnumerable<SpeciesId> ids, CancellationToken cancellationToken = default);
}
