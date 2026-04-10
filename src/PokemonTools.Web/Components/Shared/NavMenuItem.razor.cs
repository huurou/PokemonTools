using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace PokemonTools.Web.Components.Shared;

public partial class NavMenuItem : ComponentBase
{
    [Parameter] public string Href { get; set; } = "";
    [Parameter] public NavLinkMatch Match { get; set; } = NavLinkMatch.Prefix;
    [Parameter] public string Text { get; set; } = "";
    [Parameter] public RenderFragment? ChildContent { get; set; }
}
