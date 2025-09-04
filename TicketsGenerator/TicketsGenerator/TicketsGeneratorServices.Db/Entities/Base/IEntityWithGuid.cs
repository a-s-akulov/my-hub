namespace TicketsGeneratorServices.Db.Entities.Base
{

    public interface IEntityWithGuid
    {
        /// <summary>
        /// Идентификатор сущности
        /// </summary>
        public Guid Id { get; set; }
    }
}