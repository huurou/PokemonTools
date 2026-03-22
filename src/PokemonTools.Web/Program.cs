using PokemonTools.ServiceDefaults;
using PokemonTools.Web.Components;
using PokemonTools.Web.Infrastructure.Db;
using PokemonTools.Web.Infrastructure.PokeApi;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOutputCache();

builder.AddNpgsqlDbContext<PokemonToolsDbContext>(
    "pokemonToolsDb",
    configureDbContextOptions: options => options.AddInterceptors(new TimestampSaveChangesInterceptor(TimeProvider.System))
);
builder.Services.AddPokeApiClient();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseOutputCache();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();
