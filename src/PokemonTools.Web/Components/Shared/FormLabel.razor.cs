using Microsoft.AspNetCore.Components;

namespace PokemonTools.Web.Components.Shared;

public partial class FormLabel : ComponentBase
{
    [Parameter] public bool Required { get; set; }
    [Parameter] public FormLabelSize Size { get; set; } = FormLabelSize.Normal;
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private string MergedCssClass
    {
        get
        {
            var css = $"{BASE_CLASS} {SizeClass}";
            var additional = AdditionalAttributes?.GetValueOrDefault("class")?.ToString();
            return string.IsNullOrEmpty(additional) ? css : $"{css} {additional}";
        }
    }

    private Dictionary<string, object>? FilteredAttributes =>
        AdditionalAttributes
            ?.Where(x => !string.Equals(x.Key, "class", StringComparison.OrdinalIgnoreCase))
            .ToDictionary(x => x.Key, x => x.Value) is { Count: > 0 } d ? d : null;

    private const string BASE_CLASS = "inline-block mb-2 font-medium";

    private string SizeClass => Size switch
    {
        FormLabelSize.Small => "text-sm",
        _ => "",
    };
}

public enum FormLabelSize
{
    Normal,
    Small,
}
