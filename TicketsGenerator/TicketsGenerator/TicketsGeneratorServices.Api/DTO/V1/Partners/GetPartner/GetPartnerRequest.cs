namespace TicketsGeneratorServices.Api.DTO.V1.Partners
{

    public class GetPartnerRequest : ApiRequest
    {
        /// <summary>
        /// Идентификатор магазина ОРС для получения данных
        /// </summary>
        public int PartnerId { get; set; }
    }
}