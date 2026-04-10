using Microsoft.AspNetCore.Components;

namespace PokemonTools.Web.Components.Shared;

public partial class Alert : ComponentBase
{
    [Parameter] public AlertVariant Variant { get; set; } = AlertVariant.Info;
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private string MergedCssClass
    {
        get
        {
            var css = $"p-4 rounded-md border {VariantClass}";
            var additional = AdditionalAttributes?.GetValueOrDefault("class")?.ToString();
            return string.IsNullOrEmpty(additional) ? css : $"{css} {additional}";
        }
    }

    private Dictionary<string, object>? FilteredAttributes =>
        AdditionalAttributes
            ?.Where(x => !string.Equals(x.Key, "class", StringComparison.OrdinalIgnoreCase))
            .ToDictionary(x => x.Key, x => x.Value) is { Count: > 0 } d ? d : null;

    private string VariantClass => Variant switch
    {
        AlertVariant.Info => "border-[#b6effb] text-[#055160] bg-[#cff4fc]",
        AlertVariant.Success => "border-[#badbcc] text-[#0f5132] bg-[#d1e7dd]",
        AlertVariant.Warning => "border-[#ffecb5] text-[#664d03] bg-[#fff3cd]",
        AlertVariant.Danger => "border-[#f5c2c7] text-[#842029] bg-[#f8d7da]",
        _ => "",
    };
}

public enum AlertVariant
{
    Info,
    Success,
    Warning,
    Danger,
}
