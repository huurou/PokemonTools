namespace PokemonTools.Web.Domain.Individuals;

public interface IIndividualRepository
{
    /// <summary>
    /// 手持ち個体の一覧を取得します。
    /// </summary>
    Task<List<Individual>> GetOwnedIndividualsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 指定したIDの個体を取得します。
    /// </summary>
    Task<Individual?> FindByIdAsync(IndividualId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 個体を追加します。
    /// </summary>
    Task AddAsync(Individual individual, CancellationToken cancellationToken = default);

    /// <summary>
    /// 個体を更新します。
    /// </summary>
    Task UpdateAsync(Individual individual, CancellationToken cancellationToken = default);

    /// <summary>
    /// 個体を削除します。
    /// </summary>
    Task DeleteAsync(IndividualId id, CancellationToken cancellationToken = default);
}
