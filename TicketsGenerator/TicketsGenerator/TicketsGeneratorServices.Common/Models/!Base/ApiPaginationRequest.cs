namespace TicketsGeneratorServices.Common.Models.Base
{

    /// <summary>
    /// Информация о постраничности запроса к API
    /// </summary>
    public class ApiPaginationRequest
    {
        /// <summary>
        /// Номер запрашиваемой страницы (от 1)
        /// <br/>Если не указан - будет возвращена первая страница (номер 1)
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// Количество элементов на одной странице ответа (от 0)
        /// <br/>Если не указано - используется значение по умолчанию в зависимости от запрашиваемого метода
        /// </summary>
        public int? PerPage { get; set; }
    }
}