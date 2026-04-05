using PokemonTools.Web.Domain.Individuals;

namespace PokemonTools.Web.Domain.Parties;

public interface IPartyRepository
{
    /// <summary>
    /// 指定した個体がいずれかのパーティに所属しているかを確認します。
    /// </summary>
    Task<bool> IsIndividualInAnyPartyAsync(IndividualId individualId, CancellationToken cancellationToken = default);
}
