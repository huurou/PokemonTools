using Microsoft.Extensions.DependencyInjection;

namespace PokemonTools.ApiService.Infrastructure.PokeApi;

/// <summary>
/// <see cref="PokeApiClient"/> の DI 登録を提供する拡張メソッド。
/// </summary>
public static class PokeApiServiceExtensions
{
    /// <summary>
    /// <see cref="PokeApiClient"/> と <see cref="PokeApiRequestLimiter"/> を DI コンテナに登録する。
    /// </summary>
    public static IServiceCollection AddPokeApiClient(this IServiceCollection services)
    {
        services.AddSingleton<PokeApiRequestLimiter>();
        services.AddHttpClient<PokeApiClient>(client => client.BaseAddress = new Uri("https://pokeapi.co/api/v2/"));
        return services;
    }
}
