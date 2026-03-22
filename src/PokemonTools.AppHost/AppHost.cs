var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .AddDatabase("pokemonToolsDb");

builder.AddProject<Projects.PokemonTools_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(postgres)
    .WaitFor(postgres);

builder.Build().Run();
