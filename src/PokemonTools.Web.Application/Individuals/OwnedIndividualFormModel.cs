namespace PokemonTools.Web.Application.Individuals;

public class OwnedIndividualFormModel
{
    public string? Name { get; set; }

    public int SelectedSpeciesId { get; set; }

    public int SelectedStatAlignmentId { get; set; }

    public int SelectedAbilityId { get; set; }

    public uint StatPointHp { get; set; }
    public uint StatPointAttack { get; set; }
    public uint StatPointDefense { get; set; }
    public uint StatPointSpecialAttack { get; set; }
    public uint StatPointSpecialDefense { get; set; }
    public uint StatPointSpeed { get; set; }

    public int SelectedMove1Id { get; set; }
    public int SelectedMove2Id { get; set; }
    public int SelectedMove3Id { get; set; }
    public int SelectedMove4Id { get; set; }

    public int SelectedHeldItemId { get; set; }

    public int SelectedTeraTypeId { get; set; }

    public string? Memo { get; set; }

    public void ApplyDefaults(OwnedIndividualFormDataDto formData)
    {
        SelectedStatAlignmentId = formData.DefaultStatAlignmentId;
        SelectedTeraTypeId = formData.DefaultTeraTypeId;
    }

    public RegisterOwnedIndividualCommand ToRegisterCommand()
    {
        return new RegisterOwnedIndividualCommand(
            string.IsNullOrWhiteSpace(Name) ? null : Name,
            SelectedSpeciesId,
            SelectedStatAlignmentId,
            SelectedAbilityId,
            StatPointHp, StatPointAttack, StatPointDefense,
            StatPointSpecialAttack, StatPointSpecialDefense, StatPointSpeed,
            SelectedMove1Id,
            SelectedMove2Id > 0 ? SelectedMove2Id : null,
            SelectedMove3Id > 0 ? SelectedMove3Id : null,
            SelectedMove4Id > 0 ? SelectedMove4Id : null,
            SelectedHeldItemId > 0 ? SelectedHeldItemId : null,
            SelectedTeraTypeId,
            string.IsNullOrWhiteSpace(Memo) ? null : Memo
        );
    }

    public UpdateOwnedIndividualCommand ToUpdateCommand(string id)
    {
        return new UpdateOwnedIndividualCommand(
            id,
            string.IsNullOrWhiteSpace(Name) ? null : Name,
            SelectedSpeciesId,
            SelectedStatAlignmentId,
            SelectedAbilityId,
            StatPointHp, StatPointAttack, StatPointDefense,
            StatPointSpecialAttack, StatPointSpecialDefense, StatPointSpeed,
            SelectedMove1Id,
            SelectedMove2Id > 0 ? SelectedMove2Id : null,
            SelectedMove3Id > 0 ? SelectedMove3Id : null,
            SelectedMove4Id > 0 ? SelectedMove4Id : null,
            SelectedHeldItemId > 0 ? SelectedHeldItemId : null,
            SelectedTeraTypeId,
            string.IsNullOrWhiteSpace(Memo) ? null : Memo
        );
    }
}
