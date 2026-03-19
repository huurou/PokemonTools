using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace PokemonTools.ApiService.Infrastructure.PokeApi;

/// <summary>
/// PokeAPI v2 のHTTPクライアント
/// ページネーション処理とリクエスト間隔のスロットリングを提供します。
/// </summary>
internal class PokeApiClient(HttpClient httpClient, PokeApiRequestLimiter limiter)
{
    private const int PAGE_SIZE = 100;

    /// <summary>
    /// 指定エンドポイントの全リソース一覧をページネーションを辿って取得します。
    /// </summary>
    /// <param name="endpoint">PokeAPI のリソースパス (例: "pokemon", "type")</param>
    /// <param name="cancellationToken">キャンセルトークン</param>
    /// <returns>各ページの <see cref="NamedApiResource"/> を順次返す非同期シーケンス</returns>
    public async IAsyncEnumerable<NamedApiResource> GetAllResourcesAsync(
        string endpoint,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        var url = $"{endpoint}?limit={PAGE_SIZE}";

        while (url is not null)
        {
            await limiter.WaitAsync(cancellationToken);
            var response = await httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();
            var page = await response.Content.ReadFromJsonAsync<NamedApiResourceList>(cancellationToken);

            foreach (var resource in page!.Results)
            {
                yield return resource;
            }

            url = page.Next;
        }
    }

    /// <summary>
    /// 指定エンドポイントから名前または ID でリソースを個別取得します。
    /// </summary>
    /// <typeparam name="T">デシリアライズ先の型。</typeparam>
    /// <param name="endpoint">PokeAPI のリソースパス (例: "pokemon", "type")</param>
    /// <param name="nameOrId">リソースの名前または ID</param>
    /// <param name="cancellationToken">キャンセルトークン</param>
    /// <returns>デシリアライズされたリソース</returns>
    public Task<T> GetResourceAsync<T>(
        string endpoint,
        string nameOrId,
        CancellationToken cancellationToken = default
    )
    {
        return GetAsync<T>($"{endpoint}/{nameOrId}", cancellationToken);
    }

    /// <summary>
    /// <see cref="NamedApiResource"/> の URL からリソースを個別取得します。
    /// </summary>
    /// <typeparam name="T">デシリアライズ先の型</typeparam>
    /// <param name="resource">一覧取得で得られた <see cref="NamedApiResource"/></param>
    /// <param name="cancellationToken">キャンセルトークン</param>
    /// <returns>デシリアライズされたリソース</returns>
    public Task<T> GetResourceAsync<T>(
        NamedApiResource resource,
        CancellationToken cancellationToken = default
    )
    {
        ValidateResourceUrl(resource.Url);
        return GetAsync<T>(resource.Url, cancellationToken);
    }

    private async Task<T> GetAsync<T>(string url, CancellationToken cancellationToken)
    {
        await limiter.WaitAsync(cancellationToken);
        var response = await httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<T>(cancellationToken))!;
    }

    private void ValidateResourceUrl(string url)
    {
        var baseAddress = httpClient.BaseAddress?.ToString();
        if (baseAddress is not null && !url.StartsWith(baseAddress, StringComparison.Ordinal))
        {
            throw new ArgumentException($"URL must be under the base address: {baseAddress}", nameof(url));
        }
    }
}
