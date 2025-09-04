using TicketsGeneratorServices.Db.Entities.Base;


namespace TicketsGeneratorServices.Db.Entities
{
    public abstract class MyAwesomeProductBase : EntityWithGuidBase
    {
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
