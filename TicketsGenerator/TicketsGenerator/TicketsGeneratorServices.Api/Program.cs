using TicketsGeneratorServices.Api;


var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

var app = builder.Build();

app.Logger.LogInformation("--- ������ ������ TicketsGeneratorServices.Api ---");

app.ConfigureApplication();

app.Run();
