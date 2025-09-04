using TicketsGeneratorServices.Db.Entities;


namespace TicketsGeneratorServices.Api.DTO.V1.MyAwesomeProducts
{
    public class GetProductsResponse
    {
        /// <summary>
        /// Список найденных продуктов
        /// </summary>
        public ICollection<MyAwesomeProduct> Products { get; set; } = [];
    }
}
