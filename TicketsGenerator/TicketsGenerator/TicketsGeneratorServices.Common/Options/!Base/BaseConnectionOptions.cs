namespace TicketsGeneratorServices.Common.Options.Base
{

    /// <summary>
    /// Базовая конфигурации соединения
    /// </summary>
    public abstract class BaseConnectionOptions
    {
        /// <summary>
        /// Использовать подменный источник данных - без обращения в настроенный внешний сервис
        /// </summary>
        public bool UseMockup { get; init; }
    }
}
