namespace PokemonTools.Web.Application.Individuals;

public interface IOwnedIndividualQueryService
{
    Task<List<OwnedIndividualListItemDto>> GetListAsync(CancellationToken cancellationToken = default);

    Task<OwnedIndividualDetailDto?> GetDetailAsync(string id, CancellationToken cancellationToken = default);

    Task<DeleteCheckResult> CheckDeleteAsync(string id, CancellationToken cancellationToken = default);
}
