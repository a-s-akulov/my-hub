var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.TicketsGeneratorServices_Api>("apiservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.TicketsGenerator_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
