using TicketsGeneratorServices.Common.Services.PartnersService;


namespace TicketsGeneratorServices.Api.DTO.V1.Partners
{
    public class GetPartnerResponse
    {
        /// <summary>
        /// Найденный магазин
        /// </summary>
        public PartnerOrsInfo? Partner { get; set; }
    }
}
