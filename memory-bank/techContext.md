# Tech Context

## Технологии
- .NET 8 (ASP.NET Core Web API)
- MediatR, AutoMapper, Serilog, ApiVersioning, Swagger
- PDF генерация: `PDFsharp` 6.2.1 (подключена в `TicketsGeneratorServices.Api.csproj`)
- БД и общий код: проекты `TicketsGeneratorServices.*`
- Frontend: Blazor (Server/Interactive), страница `TicketsGeneratorPage.razor`

## Зависимости и ресурсы
- Ресурс `Resources.template` используется для текущей заглушки PDF.
- `template.pdf` хранится в `RequestHandlers/V1/TicketsGenerator/Generator/`.

## Пакеты NuGet (ключевые)
- PDFsharp 6.2.1
- Asp.Versioning.Mvc.ApiExplorer 8.1.0
- Microsoft.EntityFrameworkCore.Design 9.0.8 (dev)

## Замечания по PDF
- Проверить встраивание шрифтов при использовании PDFsharp.
- Для штрихкодов: рассмотреть `ZXing.Net` (рисовать в `XGraphics.DrawImage`).
