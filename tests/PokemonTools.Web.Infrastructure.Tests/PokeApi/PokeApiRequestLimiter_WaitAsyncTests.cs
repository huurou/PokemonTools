using Microsoft.Extensions.Time.Testing;
using PokemonTools.Web.Infrastructure.PokeApi;

namespace PokemonTools.Web.Infrastructure.Tests.PokeApi;

public class PokeApiRequestLimiter_WaitAsyncTests
{
    [Fact]
    public async Task 初回リクエスト_待機なしで完了する()
    {
        // Arrange
        var timeProvider = new FakeTimeProvider();
        using var limiter = new PokeApiRequestLimiter(timeProvider);

        // Act & Assert (完了すること自体が検証)
        await limiter.WaitAsync(TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task インターバル以上経過後のリクエスト_待機なしで完了する()
    {
        // Arrange
        var timeProvider = new FakeTimeProvider();
        using var limiter = new PokeApiRequestLimiter(timeProvider);
        await limiter.WaitAsync(TestContext.Current.CancellationToken);

        // Act
        timeProvider.Advance(TimeSpan.FromMilliseconds(200));

        // Assert (完了すること自体が検証)
        await limiter.WaitAsync(TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task インターバル未満でのリクエスト_残り時間まで待機する()
    {
        // Arrange
        var timeProvider = new FakeTimeProvider();
        using var limiter = new PokeApiRequestLimiter(timeProvider);
        await limiter.WaitAsync(TestContext.Current.CancellationToken);
        timeProvider.Advance(TimeSpan.FromMilliseconds(100));

        // Act
        var waitTask = limiter.WaitAsync(TestContext.Current.CancellationToken);

        // Assert
        Assert.False(waitTask.IsCompleted);

        timeProvider.Advance(TimeSpan.FromMilliseconds(100));
        await waitTask;
    }

    [Fact]
    public async Task キャンセル時_TaskCanceledExceptionがスローされる()
    {
        // Arrange
        var timeProvider = new FakeTimeProvider();
        using var limiter = new PokeApiRequestLimiter(timeProvider);
        await limiter.WaitAsync(TestContext.Current.CancellationToken);
        using var cts = new CancellationTokenSource();

        // Act
        var waitTask = limiter.WaitAsync(cts.Token);
        cts.Cancel();
        var exception = await Record.ExceptionAsync(() => waitTask);

        // Assert
        Assert.IsType<TaskCanceledException>(exception);
    }
}
