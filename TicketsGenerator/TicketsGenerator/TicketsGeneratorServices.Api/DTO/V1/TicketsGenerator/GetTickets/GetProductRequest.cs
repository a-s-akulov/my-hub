namespace TicketsGeneratorServices.Api.DTO.V1.TicketsGenerator;


public class GetTicketsRequest : ApiRequest
{
    /// <summary>
    /// Количество посетителей
    /// </summary>
    public int PersonsCount { get; set; }

    /// <summary>
    /// Желаемая дата посещения (локальная зона, если не указана — текущая дата)
    /// </summary>
    public DateTimeOffset? VisitDate { get; set; }
}