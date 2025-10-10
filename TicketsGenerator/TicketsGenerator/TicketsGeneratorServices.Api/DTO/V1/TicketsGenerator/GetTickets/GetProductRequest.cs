namespace TicketsGeneratorServices.Api.DTO.V1.TicketsGenerator;


public class GetTicketsRequest : ApiRequest
{
    /// <summary>
    /// Количество посетителей
    /// </summary>
    public int PersonsCount { get; set; }

    /// <summary>
    /// Желаемая дата посещения
    /// <br/>Если не указана — текущая дата
    /// </summary>
    public DateOnly? VisitDate { get; set; }

    /// <summary>
    /// Дата покупки билета
    /// <br/>Если не указана — Вчерашний день со случайным временем от 11:00 др 22:00
    /// </summary>
    public DateTimeOffset? SaleDate { get; set; }
}