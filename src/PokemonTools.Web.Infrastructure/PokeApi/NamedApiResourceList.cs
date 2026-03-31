using System.Collections.Immutable;

namespace PokemonTools.Web.Infrastructure.PokeApi;

/// <summary>
/// PokeAPI のページネーションレスポンス
/// </summary>
/// <param name="Count">総リソース数</param>
/// <param name="Previous">前のページの URL</param>
/// <param name="Next">次のページの URL</param>
/// <param name="Results">リソースの一覧</param>
public record NamedApiResourceList(
    int Count,
    string? Previous,
    string? Next,
    ImmutableList<NamedApiResource> Results
);
