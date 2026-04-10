using Microsoft.AspNetCore.Components;

namespace PokemonTools.Web.Components.Shared;

public partial class Button : ComponentBase
{
    [Parameter] public ButtonVariant Variant { get; set; } = ButtonVariant.Primary;
    [Parameter] public ButtonSize Size { get; set; } = ButtonSize.Normal;
    [Parameter] public string? Type { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public string? Href { get; set; }
    [Parameter] public EventCallback OnClick { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private string MergedCssClass
    {
        get
        {
            var css = $"{BASE_CLASS} {VariantClass} {SizeClass}";
            var additional = AdditionalAttributes?.GetValueOrDefault("class")?.ToString();
            return string.IsNullOrEmpty(additional) ? css : $"{css} {additional}";
        }
    }

    private Dictionary<string, object>? FilteredAttributes =>
        AdditionalAttributes
            ?.Where(x => !string.Equals(x.Key, "class", StringComparison.OrdinalIgnoreCase))
            .ToDictionary(x => x.Key, x => x.Value) is { Count: > 0 } d ? d : null;

    private const string BASE_CLASS =
        "inline-block font-normal text-center align-middle cursor-pointer select-none " +
        "leading-normal rounded-md border transition-colors no-underline " +
        "focus:shadow-[0_0_0_0.1rem_white,0_0_0_0.25rem_#258cfb] " +
        "disabled:opacity-65 disabled:pointer-events-none";

    private string VariantClass => Variant switch
    {
        ButtonVariant.Primary => "text-white bg-primary border-primary-border hover:bg-[#155a9e]",
        ButtonVariant.Secondary => "text-white bg-[#6c757d] border-[#6c757d] hover:bg-[#5a6268]",
        ButtonVariant.Danger => "text-white bg-[#dc3545] border-[#dc3545] hover:bg-[#c82333]",
        ButtonVariant.OutlinePrimary => "text-primary border-primary bg-transparent hover:text-white hover:bg-primary",
        ButtonVariant.OutlineSecondary => "text-[#6c757d] border-[#6c757d] bg-transparent hover:text-white hover:bg-[#6c757d]",
        ButtonVariant.OutlineDanger => "text-[#dc3545] border-[#dc3545] bg-transparent hover:text-white hover:bg-[#dc3545]",
        _ => "",
    };

    private string SizeClass => Size switch
    {
        ButtonSize.Small => "px-2 py-1 text-sm",
        ButtonSize.Normal => "px-3 py-1.5 text-base",
        _ => "px-3 py-1.5 text-base",
    };
}

public enum ButtonVariant
{
    Primary,
    Secondary,
    Danger,
    OutlinePrimary,
    OutlineSecondary,
    OutlineDanger,
}

public enum ButtonSize
{
    Normal,
    Small,
}
