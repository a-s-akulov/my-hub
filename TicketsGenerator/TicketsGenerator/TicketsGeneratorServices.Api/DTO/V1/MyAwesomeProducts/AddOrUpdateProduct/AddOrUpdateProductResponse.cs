using TicketsGeneratorServices.Db.Entities;


namespace TicketsGeneratorServices.Api.DTO.V1.MyAwesomeProducts
{
    public class AddOrUpdateProductResponse
    {
        /// <summary>
        /// Обновленный продукт
        /// </summary>
        public MyAwesomeProduct Product { get; set; }
    }
}
