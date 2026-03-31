namespace PokemonTools.Web.Infrastructure.PokeApi;

/// <summary>
/// PokeAPI の一覧レスポンスに含まれるリソース参照
/// 名前とリソースURLのペア
/// </summary>
public record NamedApiResource(string Name, string Url);
