var builder = DistributedApplication.CreateBuilder(args);

var webformApp = builder.AddProject<Projects.WebFormsApp>("WebFormsApp")
    .WithHttpsEndpoint(44348, isProxied: false)
    .WithOtlpExporter();

builder.AddProject<Projects.BlazorWebFormsApp>("BlazorWebFormsApp")
    .WaitFor(webformApp);

await builder.Build().RunAsync();
