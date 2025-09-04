namespace TicketsGeneratorServices.Api.DTO.V1.Partners
{

    public class GetPartnersRequest : ApiRequest
    {
        /// <summary>
        /// Фильтры запроса
        /// </summary>
        public GetPartnersRequestFilters? Filters { get; set; }
    }


    public class GetPartnersRequestFilters
    {
        /// <summary>
        /// Список идентификаторов магазинов
        /// </summary>
        public ICollection<int>? PartnersIds { get; set; }
    }
}