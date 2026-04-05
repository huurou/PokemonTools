namespace PokemonTools.Web.Application.Individuals;

public record OwnedIndividualFormDataDto(
    IReadOnlyList<SpeciesOptionDto> AllSpecies,
    IReadOnlyList<OptionDto> AllMoves,
    IReadOnlyList<OptionDto> AllItems,
    IReadOnlyList<OptionDto> AllAbilities,
    IReadOnlyList<OptionDto> AllStatAlignments,
    IReadOnlyList<OptionDto> AllTeraTypes,
    int DefaultStatAlignmentId,
    int DefaultTeraTypeId
);
