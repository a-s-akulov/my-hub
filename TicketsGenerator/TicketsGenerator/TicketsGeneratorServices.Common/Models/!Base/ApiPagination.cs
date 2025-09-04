namespace TicketsGeneratorServices.Common.Models.Base
{

    /// <summary>
    /// Информация о постраничном запросе API
    /// </summary>
    public class ApiPagination
    {
        /// <summary>
        /// Всего найдено результатов поиска
        /// </summary>
        public int Total { get; set; }


        /// <summary>
        /// Количество результатов на текущей странице
        /// </summary>
        public int Count { get; set; }


        /// <summary>
        /// Количество результатов на одну страницу
        /// </summary>
        public int PerPage { get; set; }


        /// <summary>
        /// Текущая страница результатов
        /// </summary>
        public int CurrentPage { get; set; }


        /// <summary>
        /// Всего страниц результатов
        /// </summary>
        public int TotalPages { get; set; }
    }
}