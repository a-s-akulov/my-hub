namespace TicketsGeneratorServices.Api.DTO.V1.TicketsGenerator;


public class GetTicketsRequest : ApiRequest
{
    /// <summary>
    /// Количество посетителей
    /// </summary>
    public int PersonsCount { get; set; }
}