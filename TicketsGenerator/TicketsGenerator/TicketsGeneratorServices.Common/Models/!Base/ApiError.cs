namespace TicketsGeneratorServices.Common.Models.Base
{

    /// <summary>
    /// Информация об ошибке API
    /// </summary>
    public class ApiError
    {
        /// <summary>
        /// Идентификатор ошибки
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Источник ошибки
        /// </summary>
        public string Source { get; set; } = string.Empty;

        /// <summary>
        /// Краткое описание ошибки
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Подробное описание ошибки
        /// </summary>
        public string? Detail { get; set; }
    }
}