using Hapvida.Core.Infra.OpenTelemetry.Exporter.Collector;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddAppContext<Program>(configuration);
builder.Services.AddDomainContext(configuration);

builder.Services.AddOpenTelemetryContext(configuration);

var app = builder.Build();
app.UseApplicationContext();
app.Run();