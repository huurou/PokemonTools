var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddConnectionString("pokemon-tools-db");

builder.AddProject<Projects.PokemonTools_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(db);

builder.Build().Run();
