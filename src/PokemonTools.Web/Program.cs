using Microsoft.EntityFrameworkCore;
using PokemonTools.ServiceDefaults;
using PokemonTools.Web.Application.MasterData;
using PokemonTools.Web.Components;
using PokemonTools.Web.Domain.Abilities;
using PokemonTools.Web.Domain.Items;
using PokemonTools.Web.Domain.Moves;
using PokemonTools.Web.Domain.Species;
using PokemonTools.Web.Infrastructure.Abilities;
using PokemonTools.Web.Infrastructure.Db;
using PokemonTools.Web.Infrastructure.Items;
using PokemonTools.Web.Infrastructure.Moves;
using PokemonTools.Web.Infrastructure.PokeApi;
using PokemonTools.Web.Infrastructure.Species;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOutputCache();

builder.AddNpgsqlDbContext<PokemonToolsDbContext>(
    "pokemon-tools-db",
    configureDbContextOptions: options => options.AddInterceptors(new TimestampSaveChangesInterceptor(TimeProvider.System))
);

builder.Services.AddPokeApiClient();

builder.Services.AddScoped<MasterDataImportUseCase>();
builder.Services.AddScoped<IAbilityDataFetcher, AbilityDataFetcher>();
builder.Services.AddScoped<IItemDataFetcher, ItemDataFetcher>();
builder.Services.AddScoped<IMoveDataFetcher, MoveDataFetcher>();
builder.Services.AddScoped<ISpeciesDataFetcher, SpeciesDataFetcher>();
builder.Services.AddScoped<IAbilityRepository, AbilityRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IMoveRepository, MoveRepository>();
builder.Services.AddScoped<ISpeciesRepository, SpeciesRepository>();


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

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PokemonToolsDbContext>();
    await dbContext.Database.MigrateAsync();
}

await app.RunAsync();
