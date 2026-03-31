using PokemonTools.Web.Domain.Types;

namespace PokemonTools.Web.Domain.Moves;

/// <summary>
/// 技を表現するクラス
/// </summary>
public record Move
{
    /// <summary>
    /// 技Id
    /// </summary>
    public MoveId Id { get; init; }

    /// <summary>
    /// 技名
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// タイプId
    /// </summary>
    public TypeId TypeId
    {
        get;
        init
        {
            if (!PokemonType.All.Any(x => x.Id == value))
            {
                throw new ArgumentException("技のタイプは有効なタイプを指定してください。", nameof(TypeId));
            }
            field = value;
        }
    }

    /// <summary>
    /// 技分類Id
    /// </summary>
    public MoveDamageClassId DamageClassId
    {
        get;
        init
        {
            if (!MoveDamageClass.All.Any(x => x.Id == value))
            {
                throw new ArgumentException("無効な技分類Idです。", nameof(DamageClassId));
            }
            field = value;
        }
    }

    /// <summary>
    /// 威力 へんか技等はnull
    /// </summary>
    public uint? Power { get; init; }

    public Move(MoveId id, string name, TypeId typeId, MoveDamageClassId damageClassId, uint? power)
    {
        Id = id;
        Name = name;
        TypeId = typeId;
        DamageClassId = damageClassId;
        Power = power;
    }
}
