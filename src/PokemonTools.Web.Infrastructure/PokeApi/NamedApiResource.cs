namespace PokemonTools.Web.Infrastructure.PokeApi;

/// <summary>
/// PokeAPI の一覧レスポンスに含まれるリソース参照
/// 名前とリソースURLのペア
/// </summary>
internal record NamedApiResource(string Name, string Url);
