namespace TicketsGeneratorServices.Db.Entities.Base
{

    public interface IEntityWithLogs<TLogEntity> where TLogEntity : class, ILogEntity
    {
        /// <summary>
        /// Логи сущности
        /// </summary>
        public ICollection<TLogEntity> Logs { get; set; }
    }
}