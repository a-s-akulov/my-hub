namespace TicketsGeneratorServices.Common.Options.Base
{

    /// <summary>
    /// Конфигурация соединения с БД
    /// </summary>
    public class DbConnectionOptions : BaseConnectionOptions
    {
        /// <summary>
        /// Базовая строка подключения к БД
        /// </summary>
        public string ConnectionString { get; init; } = string.Empty;

        /// <summary>
        /// Имя пользователя для подключения БД
        /// </summary>
        public string Username { get; init; } = string.Empty;

        /// <summary>
        /// Пароль для подключения БД
        /// </summary>
        public string Password { get; init; } = string.Empty;
    }
}
