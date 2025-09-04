using TicketsGeneratorServices.Db.Entities.Base;


namespace TicketsGeneratorServices.Db.Entities
{
    public class MyAwesomeProduct : MyAwesomeProductBase, IEntityWithLogs<LogMyAwesomeProduct>
    {
        /// <inheritdoc/>>
        public ICollection<LogMyAwesomeProduct> Logs { get; set; } = [];
    }
}