namespace PokemonTools.Web.Infrastructure.PokeApi.Responses;

public static class PokeApiUrlHelper
{
    public static int ExtractIdFromUrl(string url)
    {
        var trimmed = url.TrimEnd('/');
        var lastSlash = trimmed.LastIndexOf('/');
        return int.Parse(trimmed[(lastSlash + 1)..]);
    }
}
