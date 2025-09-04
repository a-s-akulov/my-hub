using TicketsGeneratorServices.ScheduleJobs;


var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

var app = builder.Build();

app.Logger.LogInformation("--- Запуск службы TicketsGeneratorServices.ScheduleJobs ---");

app.ConfigureApplication();

app.Run();