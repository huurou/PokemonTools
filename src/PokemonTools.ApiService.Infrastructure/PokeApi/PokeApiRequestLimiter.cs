namespace PokemonTools.ApiService.Infrastructure.PokeApi;

/// <summary>
/// PokeAPI フェアユースポリシー遵守のためのリクエスト間隔リミッター
/// リクエスト開始間隔を 200ms 以上に保証する。シングルトンとして登録します。
/// </summary>
internal sealed class PokeApiRequestLimiter(TimeProvider timeProvider) : IDisposable
{
    private static readonly TimeSpan RequestInterval = TimeSpan.FromMilliseconds(200);
    private readonly SemaphoreSlim semaphore_ = new(1, 1);
    private long lastRequestTick_;

    /// <summary>
    /// 前回リクエストから所定の間隔が経過するまで待機します。
    /// </summary>
    public async Task WaitAsync(CancellationToken cancellationToken)
    {
        await semaphore_.WaitAsync(cancellationToken);
        try
        {
            var now = timeProvider.GetTimestamp();
            var elapsed = timeProvider.GetElapsedTime(lastRequestTick_, now);
            if (lastRequestTick_ != 0 && elapsed < RequestInterval)
            {
                await Task.Delay(RequestInterval - elapsed, timeProvider, cancellationToken);
            }
            lastRequestTick_ = timeProvider.GetTimestamp();
        }
        finally
        {
            semaphore_.Release();
        }
    }

    public void Dispose()
    {
        semaphore_.Dispose();
    }
}
