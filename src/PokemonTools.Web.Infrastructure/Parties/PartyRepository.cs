using Microsoft.EntityFrameworkCore;
using PokemonTools.Web.Domain.Individuals;
using PokemonTools.Web.Domain.Parties;
using PokemonTools.Web.Infrastructure.Db;

namespace PokemonTools.Web.Infrastructure.Parties;

public class PartyRepository(PokemonToolsDbContext context) : IPartyRepository
{
    public async Task<bool> IsIndividualInAnyPartyAsync(IndividualId individualId, CancellationToken cancellationToken = default)
    {
        var id = individualId.Value;
        return await context.Parties.AnyAsync(x =>
            x.Individual1Id == id || x.Individual2Id == id || x.Individual3Id == id ||
            x.Individual4Id == id || x.Individual5Id == id || x.Individual6Id == id,
            cancellationToken);
    }
}
