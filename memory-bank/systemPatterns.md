# System Patterns

## Архитектура
- Backend: ASP.NET Core 8, MediatR, слои: Controllers → RequestHandlers → Services.
- Обработчики наследуются от `RequestHandlerBase<TBaseService, TRequest, TResponse>` и возвращают `ApiResponse<T>`.
- Конфигурация через `Startup.ConfigureServices` и `ConfigureApplication`.

## Поток генерации билетов
1. `TicketsGeneratorV1Controller.GetTickets` принимает `GetTicketsRequest` (query).
2. Посылает запрос через Mediatr к `GetTicketsV1RequestHandler`.
3. Handler формирует `FileStream` PDF и возвращает `GetTicketsResponse` с метаданными файла.

## Шаблоны и ресурсы
- Пример билетов `RequestHandlers/V1/TicketsGenerator/Generator/template.pdf`.
- Встроенные ресурсы (`Resources.resx`) могут содержать байты шаблона (сейчас `Resources.template`).

## План внедрения PDF генерации
- Реализовать `GenerateDocument(int personsCount, DateTimeOffset visitDate)` с `PdfSharp`.
- Разметка страниц и повторение блока билета по количеству.
- Генерация номера заказа и штрихкода, отрисовка поверх холста PDF.

## Ошибки/исключения
- Обработчики уже имеют унифицированный try/catch (см. `RequestHandlerBaseT`).
- При ошибке генерации возвращать 500 с деталями через `ToApiErrors`.
