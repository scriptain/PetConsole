var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.PetStoreApp_ApiService>("apiservice");

builder.AddProject<Projects.PetStoreApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
