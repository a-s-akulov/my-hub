

namespace TicketsGeneratorServices.Db.Entities.Base
{

    /// <summary>
    /// Тип операции логируемых данных
    /// </summary>
    public enum enLogOperation
    {
        /// <summary>
        /// Создание записи
        /// </summary>
        Add = 1,

        /// <summary>
        /// Обновление записи
        /// </summary>
        Update = 2,

        /// <summary>
        /// Удаление записи
        /// </summary>
        Remove = 3
    }
}
