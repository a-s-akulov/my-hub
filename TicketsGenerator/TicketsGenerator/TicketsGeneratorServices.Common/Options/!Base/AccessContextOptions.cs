namespace TicketsGeneratorServices.Common.Options.Base
{

    /// <summary>
    /// Конфигурация корпоративного сервиса аутентификации AccessAPI
    /// </summary>
    public record AccessContextOptions
    {
        /// <summary>
        /// УРЛ доступа по паролю
        /// </summary>
        public string UrlPasswordAuth { get; init; }

        /// <summary>
        /// Урл доступа по учетной записи ОС
        /// </summary>
        public string UrlWindowsAuth { get; init; }

        /// <summary>
        /// Идентификатор приложения
        /// </summary>
        public string ApplicationName { get; init; }
    }
}