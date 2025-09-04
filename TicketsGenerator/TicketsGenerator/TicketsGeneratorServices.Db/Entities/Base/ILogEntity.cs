namespace TicketsGeneratorServices.Db.Entities.Base
{

    public interface ILogEntity
    {
        /// <summary>
        /// ID записи лога
        /// </summary>
        public Guid LogId { get; set; }

        /// <summary>
        /// Дата и время изменения сущности
        /// </summary>
        public DateTime ChangedDate { get; set; }

        /// <summary>
        /// Тип операции изменения сущности
        /// </summary>
        public enLogOperation ChangedOperation { get; set; }
    }


    public interface ILogEntity<TEntity> : ILogEntity
    {
        /// <summary>
        /// Оригинальная сущность, для которой записан лог
        /// </summary>
        public TEntity? Entity { get; set; }
    }
}