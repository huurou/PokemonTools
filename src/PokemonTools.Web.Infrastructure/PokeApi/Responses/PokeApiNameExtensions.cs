namespace PokemonTools.Web.Infrastructure.PokeApi.Responses;

public static class PokeApiNameExtensions
{
    private const string JAPANESE_LANGUAGE = "ja";
    private const string JAPANESE_LANGUAGE_HRKT = "ja-hrkt";
    private const string ENGLISH_LANGUAGE = "en";

    public static string GetName(this IEnumerable<PokeApiLocalizedName> names, string fallbackName)
    {
        return names.FirstOrDefault(x => x.Language.Name == JAPANESE_LANGUAGE)?.Name
            ?? names.FirstOrDefault(x => x.Language.Name == JAPANESE_LANGUAGE_HRKT)?.Name
            ?? names.FirstOrDefault(x => x.Language.Name == ENGLISH_LANGUAGE)?.Name
            ?? fallbackName;
    }
}
