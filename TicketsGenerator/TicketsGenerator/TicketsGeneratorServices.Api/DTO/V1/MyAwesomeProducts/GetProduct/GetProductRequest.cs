namespace TicketsGeneratorServices.Api.DTO.V1.MyAwesomeProducts
{

    public class GetProductRequest : ApiRequest
    {
        /// <summary>
        /// ID Продукта
        /// </summary>
        public Guid Id { get; set; }
    }
}