namespace PokemonTools.Web.Application.Individuals;

public interface IOwnedIndividualFormQueryService
{
    Task<OwnedIndividualFormDataDto> GetFormDataAsync(CancellationToken cancellationToken = default);
}
