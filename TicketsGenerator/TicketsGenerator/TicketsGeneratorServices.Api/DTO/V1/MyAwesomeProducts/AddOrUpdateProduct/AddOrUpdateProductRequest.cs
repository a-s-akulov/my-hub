using TicketsGeneratorServices.Db.Entities;


namespace TicketsGeneratorServices.Api.DTO.V1.MyAwesomeProducts
{
    public class AddOrUpdateProductRequest : ApiRequest
    {
        /// <summary>
        /// Продукт для создания/обновления
        /// </summary>
        public MyAwesomeProductToPut Product { get; set; }
    }


    public class MyAwesomeProductToPut
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название продукта
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Тип продукта
        /// </summary>
        public enAwesomeProductType ProductType { get; set; }
    }
}