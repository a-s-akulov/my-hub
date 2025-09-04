using TicketsGeneratorServices.Db.Entities;


namespace TicketsGeneratorServices.Api.DTO.V1.MyAwesomeProducts
{
    public class GetProductResponse
    {
        /// <summary>
        /// Найденный продукт
        /// </summary>
        public MyAwesomeProduct? Product { get; set; }
    }
}
