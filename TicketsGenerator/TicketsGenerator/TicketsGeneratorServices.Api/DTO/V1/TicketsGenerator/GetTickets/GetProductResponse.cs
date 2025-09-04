namespace TicketsGeneratorServices.Api.DTO.V1.TicketsGenerator;


public class GetTicketsResponse
{
    public Stream FileStream { get; set; }

    public string ContentType { get; set; }

    public string DownloadFileName { get; set; }
}