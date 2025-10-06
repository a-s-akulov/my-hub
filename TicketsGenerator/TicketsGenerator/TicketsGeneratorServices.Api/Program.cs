using TicketsGeneratorServices.Api;
using PdfSharp.Fonts;
using TicketsGeneratorServices.Api.Configuration.Fonts;


var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

// Регистрация резолвера шрифтов PDFsharp до любой генерации PDF
GlobalFontSettings.FontResolver ??= new EmbeddedFontResolver();

var app = builder.Build();

app.Logger.LogInformation("--- ������ ������ TicketsGeneratorServices.Api ---");

app.ConfigureApplication();

app.Run();
