using TicketsGeneratorServices.Api;


var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

var app = builder.Build();

app.Logger.LogInformation("--- Запуск службы TicketsGeneratorServices.Api ---");

app.ConfigureApplication();

app.Run();
