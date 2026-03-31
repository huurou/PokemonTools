using Microsoft.EntityFrameworkCore;
using PokemonTools.Web.Domain.Moves;
using PokemonTools.Web.Domain.Types;
using PokemonTools.Web.Infrastructure.Moves;

namespace PokemonTools.Web.Infrastructure.Tests.Moves;

public class MoveRepository_UpsertRangeAsyncTests(PostgreSqlFixture fixture) : IClassFixture<PostgreSqlFixture>
{
    [Fact]
    public async Task 新規技_DBに挿入される()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var context = fixture.CreateContext();
        var repository = new MoveRepository(context);
        var moves = new List<Move>
        {
            new(new MoveId(1), "はたく", PokemonType.Normal.Id, MoveDamageClass.Physical.Id, 40),
            new(new MoveId(14), "つるぎのまい", PokemonType.Normal.Id, MoveDamageClass.Status.Id, null),
        };

        // Act
        await repository.UpsertRangeAsync(moves, ct);

        // Assert
        await using var verifyContext = fixture.CreateContext();
        var stored = await verifyContext.Moves
            .Where(x => x.MoveId == 1 || x.MoveId == 14)
            .OrderBy(x => x.MoveId)
            .ToListAsync(ct);
        Assert.Equal(2, stored.Count);
        Assert.Equal("はたく", stored[0].MoveName);
        Assert.Equal(PokemonType.Normal.Id.Value, stored[0].TypeId);
        Assert.Equal(MoveDamageClass.Physical.Id.Value, stored[0].MoveDamageClassId);
        Assert.Equal(40, stored[0].Power);
        Assert.Equal("つるぎのまい", stored[1].MoveName);
        Assert.Null(stored[1].Power);
    }

    [Fact]
    public async Task 既存技を更新_各プロパティが上書きされる()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var setupContext = fixture.CreateContext();
        var setupRepo = new MoveRepository(setupContext);
        await setupRepo.UpsertRangeAsync([new Move(new MoveId(400), "旧名", PokemonType.Normal.Id, MoveDamageClass.Physical.Id, 50)], ct);

        await using var context = fixture.CreateContext();
        var repository = new MoveRepository(context);

        // Act
        await repository.UpsertRangeAsync([new Move(new MoveId(400), "新名", PokemonType.Fire.Id, MoveDamageClass.Special.Id, 90)], ct);

        // Assert
        await using var verifyContext = fixture.CreateContext();
        var entity = await verifyContext.Moves.SingleAsync(x => x.MoveId == 400, ct);
        Assert.Equal("新名", entity.MoveName);
        Assert.Equal(PokemonType.Fire.Id.Value, entity.TypeId);
        Assert.Equal(MoveDamageClass.Special.Id.Value, entity.MoveDamageClassId);
        Assert.Equal(90, entity.Power);
    }

    [Fact]
    public async Task 空リスト_例外なく完了する()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var context = fixture.CreateContext();
        var repository = new MoveRepository(context);

        // Act
        await repository.UpsertRangeAsync([], ct);

        // Assert（例外が発生しなければ成功）
    }
}
